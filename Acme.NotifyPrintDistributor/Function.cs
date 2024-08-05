using Acme.CodeTest.Api.Api;
using Acme.CodeTest.Api.Client;
using Acme.NotifyPrintDistributor.Interfaces;
using Acme.NotifyPrintDistributor.Model;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Newtonsoft.Json;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System.Net;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Acme.NotifyPrintDistributor;

public class Function
{

    #region Fields

    /// <summary>
    /// Di container
    /// </summary>
    private readonly Container _container;

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor called by an actual call from AWS Lambda.
    /// </summary>
    public Function() : this(null)
    {
    }

    /// <summary>
    /// Constructor allowing controllers to be passed in. Used for testing.
    /// </summary>
    /// <param name="container">Di Container</param>
    public Function(Container container)
    {
        _container = container ?? DiConfig.Configure();
    }

    #endregion


    /// <summary>
    /// Function handler and entry point
    /// </summary>
    /// <param name="sqsEvent">Sqs event</param>
    /// <param name="context">Lambda cointext</param>    
    public async Task FunctionHandler(SQSEvent sqsEvent, ILambdaContext context)
    {
        IDictionary<int, ISubscriptionHandler> distibutorHandlers;        
        ISubscriptionApi subscriptionApi;
        ISubscriptionHandler subscriptionHandler;
        SourceRecord sourceRecord = null;

        // Start Di scope to ensure each run instantiates new classes correctly for warm/cold starts
        using (Scope scope = AsyncScopedLifestyle.BeginScope(_container))
        {            
            foreach (SQSEvent.SQSMessage record in sqsEvent.Records)
            {
                try
                {
                    // Log entry
                    LambdaLogger.Log($"[INFO] Processing sqsEvent with body of {record.Body} ...");
                    sourceRecord = JsonConvert.DeserializeObject<SourceRecord>(record.Body);

                    // Verify source record
                    ValidateSourceRecord(sourceRecord);

                    // Get and configure our subscription api.
                    // Note: Normall of course there would be some authentication on this. Out of scope.
                    subscriptionApi = scope.GetInstance<ISubscriptionApi>();                                        

                    // Start by getting our subscription                    
                    var customerSubscription = await subscriptionApi.GetItemByIdAsync(sourceRecord.SubscriptionId);
                    int? distributorId = customerSubscription.Publication?.PrintDistributorId;

                    // Find our handler based on the distributor used in our publication
                    distibutorHandlers = scope.GetInstance<IDictionary<int, ISubscriptionHandler>>();
                    if (distributorId.HasValue && (distibutorHandlers?.ContainsKey(distributorId.Value) ?? false))
                        throw new KeyNotFoundException($"Could not determine handler for distributorId " +
                            $"{distributorId}");

                    // Call the handler
                    subscriptionHandler = distibutorHandlers[distributorId.Value];
                    await subscriptionHandler.HandleSubscriptionAsync(sourceRecord, customerSubscription);                    

                }
                catch (Exception ex)
                {
                    // Check for a not found error specifically and exit gracefully if this has occurred.
                    if (ex is ApiException && (ex as ApiException).ErrorCode == (int)HttpStatusCode.NotFound)
                    {
                        LambdaLogger.Log($"Could not find subscription with Id {sourceRecord?.SubscriptionId}");
                        return;
                    }

                    // Otherwise Log and rethrow
                    LambdaLogger.Log($"Error while processing the SQS event record. {ex}: {record.Body}");
                    throw;
                }
            }

        }
    }

    /// <summary>
    /// Validate input record
    /// </summary>
    /// <param name="sourceRecord">Source record</param>
    private void ValidateSourceRecord(SourceRecord sourceRecord)
    {
        // Validate any input here. Just dealing with an int.
        // Pretty straight forward - nothing to verify

        return;
    }
}