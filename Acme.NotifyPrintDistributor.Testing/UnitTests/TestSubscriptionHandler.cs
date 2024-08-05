using Acme.CodeTest.Api.Client;
using Acme.NotifyPrintDistributor.Handlers;
using Acme.NotifyPrintDistributor.Model;
using FirstPrintDistributorApi.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net;

namespace Acme.NotifyPrintDistributor.Testing.UnitTests
{
    [TestClass]
    public class TestSubscriptionHandler : BaseTest
    {

        /// <summary>
        /// Sample test
        /// </summary>        
        [TestMethod]
        public async Task TestMissingSubscriptionThrowsException()
        {
            Function f = new Function(_testContainer);
            SourceRecord input = new SourceRecord() { SubscriptionId = 1 };

            // Set up api response
            _mockSubscriptionApi.Setup(x => x.GetItemByIdAsync(It.IsAny<int>(), default, default))                
                .ThrowsAsync(new ApiException() { ErrorCode = (int)HttpStatusCode.NotFound });

            // Call our function
            f.FunctionHandler(GetSqsEvent(input), null);

            // Assert we tried to get the subscription
            _mockSubscriptionApi.Verify(x => x.GetItemByIdAsync(It.IsAny<int>(), default, default), Times.Once);

            // Assert nothing was sent to our handlers
            _mockFirstPrintDistributorApi.Verify(x => 
                x.CreateNewPublicationAsync(It.IsAny<FirstPrintPublicationModel>()), Times.Never);

            _mockSecondPrintDistributorApi.Verify(x =>
                x.CreatePublicationAsync(It.IsAny<SecondPrintPublicationModel>()), Times.Never);

        }
        
    }
}
