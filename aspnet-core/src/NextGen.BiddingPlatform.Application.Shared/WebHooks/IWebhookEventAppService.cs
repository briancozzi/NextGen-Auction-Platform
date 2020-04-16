using System.Threading.Tasks;
using Abp.Webhooks;

namespace NextGen.BiddingPlatform.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
