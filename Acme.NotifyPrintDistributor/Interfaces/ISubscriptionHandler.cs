using Acme.CodeTest.Api.Model;
using Acme.NotifyPrintDistributor.Model;

namespace Acme.NotifyPrintDistributor.Interfaces
{
    public interface ISubscriptionHandler
    {
        Task HandleSubscriptionAsync(SourceRecord sourceRecord, CustomerSubscriptionDto subscription);
        Type ModelType { get; set; }
        Task ConfigureExternalApiAsync();
        Task<object> GetExistingExternalPublicationByIdAsync(string externalId);
        Task SendSubscriptionToDistributorAsync(object model);
    }
}
