using Acme.CodeTest.Api.Api;
using Acme.NotifyPrintDistributor.Handlers.FirstPrintDistributor;
using Acme.NotifyPrintDistributor.Handlers.SecondPrintDistributor;
using Acme.NotifyPrintDistributor.Interfaces;
using Acme.NotifyPrintDistributor.Model;
using Amazon.Lambda.SQSEvents;
using FirstPrintDistributorApi;
using Moq;
using Newtonsoft.Json;
using SimpleInjector;

namespace Acme.NotifyPrintDistributor.Testing
{
    public class BaseTest
    {
        protected Container _testContainer;
        protected MockRepository _mockRepository;
        protected Mock<ISubscriptionApi> _mockSubscriptionApi;
        protected Mock<IFirstPrintDistributorApi> _mockFirstPrintDistributorApi;
        protected Mock<ISecondPrintDistributorApi> _mockSecondPrintDistributorApi;

        /// <summary>
        /// Constructor
        /// </summary>
        public BaseTest()
        {
            SetupEnvironmentVariables();
            SetupMocks();
            SetupDiContainer();
        }

        /// <summary>
        /// Setup environment variables
        /// </summary>
        private void SetupEnvironmentVariables()
        {
            // Environment variables contain things like external urls, Ids,
            // usernames, paths to contain secrets etc
        }

        /// <summary>
        /// Setup mocks
        /// </summary>
        private void SetupMocks()
        {
            _mockRepository = new MockRepository(MockBehavior.Default);
            _mockSubscriptionApi = _mockRepository.Create<ISubscriptionApi>();
            _mockFirstPrintDistributorApi = _mockRepository.Create<IFirstPrintDistributorApi>();
            _mockSecondPrintDistributorApi = _mockRepository.Create<ISecondPrintDistributorApi>();
        }

        /// <summary>
        /// Set up test container
        /// </summary>
        private void SetupDiContainer()
        {
            List<ISubscriptionHandler> handlers = new List<ISubscriptionHandler>();

            _testContainer = new Container();

            var mapper = MappingConfig.GetMapper();
            _testContainer.RegisterSingleton(() => mapper);
            _testContainer.Register(() => _mockSubscriptionApi.Object);
            _testContainer.Register(() => _mockFirstPrintDistributorApi.Object);
            _testContainer.Register(() => _mockSecondPrintDistributorApi.Object);            
        }

        /// <summary>
        /// Get sqs event for the given input
        /// </summary>
        /// <param name="input">Input recod</param>
        /// <returns>Sqs event</returns>        
        protected SQSEvent GetSqsEvent(SourceRecord input)
        {
            SQSEvent result = new SQSEvent();
            result.Records = new List<SQSEvent.SQSMessage>();
            result.Records.Add(new SQSEvent.SQSMessage() { Body = JsonConvert.SerializeObject(input) });

            return result;
        }
    }
}
