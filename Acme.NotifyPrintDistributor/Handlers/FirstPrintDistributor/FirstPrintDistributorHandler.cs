using Acme.CodeTest.Api.Model;
using Acme.NotifyPrintDistributor.Interfaces;
using Amazon.Lambda.Core;
using AutoMapper;
using FirstPrintDistributorApi;
using FirstPrintDistributorApi.Model;


namespace Acme.NotifyPrintDistributor.Handlers.FirstPrintDistributor
{
    public class FirstPrintDistributorHandler : BaseHandler, ISubscriptionHandler
    {

        #region Fields
        /// <summary>
        /// External api
        /// </summary>
        private IFirstPrintDistributorApi _externalApi;

        /// <summary>
        /// Model type
        /// </summary>
        public override Type ModelType { get; set; }

        /// <summary>
        /// Distributor Id matching database primary key
        /// </summary>
        public int DistributorId { get { return 1; } }
        
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper">IMapper</param>
        /// <param name="externalApi">External api</param>
        public FirstPrintDistributorHandler(IMapper mapper, IFirstPrintDistributorApi externalApi) : base(mapper)
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
            FirstPrintPublicationModel result = null;
            
            try
            {
                // Use this method to implement any business logic or data transformation specific
                // to this print distributor.

                // For example, perhaps our external Id is expected to be an integer
                int intExternalId;
                if (!int.TryParse(externalId, out intExternalId))
                    throw new InvalidCastException($"Could not cast externalId {externalId} to an integer");

                // Get any existing publication
                await _externalApi.GetPublicationInformationAsync(intExternalId);
            }
            catch(Exception ex)
            {                
                LambdaLogger.Log($"Error retrieving first print distributor's existing data: {ex}");
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

                await _externalApi.CreateNewPublicationAsync((FirstPrintPublicationModel)model);
            }
            catch (Exception ex)
            {
                LambdaLogger.Log($"Error retrieving first print distributor's existing data: {ex}");
            }
        }
    }

    public class FirstPrintDistributorMappingProfile : Profile
    {
        public FirstPrintDistributorMappingProfile()
        {
            CreateMap<CustomerSubscriptionDto, FirstPrintPublicationModel>();
        }
    }
}
