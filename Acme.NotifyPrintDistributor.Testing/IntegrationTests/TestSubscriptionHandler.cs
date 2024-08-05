using Acme.NotifyPrintDistributor.Model;
using Amazon.Lambda.SQSEvents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.NotifyPrintDistributor.Testing.IntegrationTests
{
    [TestClass]
    public class TestSubscriptionHandler : BaseTest
    {

        [TestMethod]
        public async Task TestHandler()
        {
            Function f = new Function();
            SourceRecord input = new SourceRecord();

            // Set your subscription Id here
            input.SubscriptionId = 1;

            // Run the function
            await f.FunctionHandler(GetSqsEvent(input), null);
        }

        
    }
}
