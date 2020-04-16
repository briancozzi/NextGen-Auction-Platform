using System.Threading.Tasks;
using Abp.Application.Services;

namespace NextGen.BiddingPlatform.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}
