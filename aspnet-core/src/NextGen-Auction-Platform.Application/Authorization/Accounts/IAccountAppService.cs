using System.Threading.Tasks;
using Abp.Application.Services;
using NextGen-Auction-Platform.Authorization.Accounts.Dto;

namespace NextGen-Auction-Platform.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
