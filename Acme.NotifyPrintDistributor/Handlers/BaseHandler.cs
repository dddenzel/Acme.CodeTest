using Acme.CodeTest.Api.Model;
using Acme.NotifyPrintDistributor.Model;
using Amazon.Lambda.Core;
using AutoMapper;

namespace Acme.NotifyPrintDistributor.Handlers
{
    public abstract class BaseHandler
    {
        #region Fields
        
        protected IMapper _mapper;

        #endregion
        /// <summary>
        /// Default constructor
        /// </summary>        
        /// <param name="mapper">Automapper implementation</param>
        public BaseHandler(IMapper mapper)
        {            
            _mapper = mapper;
        }

        #region Default handler logic

        /// <summary>
        /// Default handler logic
        /// </summary>
        /// <param name="sourceRecord">Source record</param>
        /// <param name="subscription">Customer subscription</param>
        public async Task HandleSubscriptionAsync(SourceRecord sourceRecord, CustomerSubscriptionDto subscription)
        {
            // Configure external api
            await ConfigureExternalApiAsync();

            // Check to see if we haven't already published subscription to the given print distributor
            var existingExternalSubscription = await
                GetExistingExternalPublicationByIdAsync(subscription.ExternalId);

            // If we have already published our subscription, log and exit
            if (existingExternalSubscription != null)
            {
                LambdaLogger.Log($"Subscription Id {sourceRecord.SubscriptionId} has alredy been " + 
                    $"sent to distributor {subscription.Publication.PrintDistributer.Name}");
                return;
            }

            // Map to external/outbound record
            object externalSubscriptionModel = MapToExternalSubscriptionModel(subscription);

            // Send to our distributor
            await SendSubscriptionToDistributorAsync(externalSubscriptionModel);

        }

        /// <summary>
        /// Map to our external model
        /// </summary>
        /// <param name="subscription">Customer subscription</param>
        /// <returns>Mapped object</returns>
        protected virtual object MapToExternalSubscriptionModel(CustomerSubscriptionDto subscription)
        {
            return _mapper.Map(subscription, subscription.GetType(), ModelType);
        }

        #endregion


        #region Abstract methods

        public abstract Type ModelType { get; set; }

        public abstract Task ConfigureExternalApiAsync();

        public abstract Task<object> GetExistingExternalPublicationByIdAsync(string externalId);

        public abstract Task SendSubscriptionToDistributorAsync(object model);

        #endregion
    }
}
