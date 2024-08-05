using Amazon.Lambda.Core;
using AutoMapper;
using FirstPrintDistributorApi;
using Acme.NotifyPrintDistributor.Interfaces;
using FirstPrintDistributorApi.Model;
using Acme.CodeTest.Api.Model;

namespace Acme.NotifyPrintDistributor.Handlers.SecondPrintDistributor
{
    public class SecondPrintDistributorHandler : BaseHandler, ISubscriptionHandler
    {
        #region Fields
        /// <summary>
        /// External api
        /// </summary>
        private ISecondPrintDistributorApi _externalApi;

        /// <summary>
        /// Model type
        /// </summary>
        public override Type ModelType { get; set; }

        /// <summary>
        /// Distributor Id matching database primary key
        /// </summary>
        public int DistributorId { get { return 2; } }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper">IMapper</param>
        /// <param name="externalApi">External api</param>
        public SecondPrintDistributorHandler(IMapper mapper, ISecondPrintDistributorApi externalApi) : base(mapper)
        {
            _externalApi = externalApi;
        }

        /// <summary>
        /// Configure authentication for our external api
        /// </summary>        
        public override async Task ConfigureExternalApiAsync()
        {
            // Configure our external Api. This will be different on a per case basis.
            // Maybe a jwt, maybe a username password etc

            return;
        }

        /// <summary>
        /// Get existing publication by external Id
        /// </summary>
        /// <param name="externalId">External Id</param>
        /// <returns>External publication information</returns>        
        public override async Task<object> GetExistingExternalPublicationByIdAsync(string externalId)
        {
            SecondPrintPublicationModel result = null;

            try
            {
                // Use this method to implement any business logic or data transformation specific
                // to this print distributor.

                // For example, perhaps our external Id is expected to be a guid
                Guid gExternalId;
                if (!Guid.TryParse(externalId, out gExternalId))
                    throw new InvalidCastException($"Could not cast externalId {externalId} to a guid");

                // Get any existing publication
                await _externalApi.GetPublicationByIdAsync(gExternalId);
            }
            catch (Exception ex)
            {
                LambdaLogger.Log($"Error retrieving Second print distributor's existing data: {ex}");
            }

            return result;
        }

        /// <summary>
        /// Send subscription to distributor
        /// </summary>
        /// <param name="model">External publication model</param>        
        public override async Task SendSubscriptionToDistributorAsync(object model)
        {
            try
            {
                // Use this method to implement any business logic or data transformation specific
                // to this print distributor

                await _externalApi.CreatePublicationAsync((SecondPrintPublicationModel)model);
            }
            catch (Exception ex)
            {
                LambdaLogger.Log($"Error retrieving Second print distributor's existing data: {ex}");
            }
        }

        /// <summary>
        /// Override mapping logic for this distributor
        /// </summary>
        /// <param name="subscription">Customer subscription</param>
        /// <returns>SecondPrintPublicationModel</returns>
        protected override object MapToExternalSubscriptionModel(CustomerSubscriptionDto subscription)
        {
            // Use this method to implement any mapping logic specific to this print distributor
            SecondPrintPublicationModel model = (SecondPrintPublicationModel)base.MapToExternalSubscriptionModel(subscription);
            model.ContentUri = $"https://SecondPrintDistribution.com/content/" + subscription.ExternalId;

            return model;
        }
        
    }

    public class SecondPrintDistributorMappingProfile : Profile
    {
        public SecondPrintDistributorMappingProfile()
        {
            CreateMap<CustomerSubscriptionDto, SecondPrintPublicationModel>();
        }
    }
}
