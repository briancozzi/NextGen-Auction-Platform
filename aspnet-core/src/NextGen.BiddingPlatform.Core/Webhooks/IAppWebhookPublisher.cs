using System.Threading.Tasks;
using NextGen.BiddingPlatform.Authorization.Users;

namespace NextGen.BiddingPlatform.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
