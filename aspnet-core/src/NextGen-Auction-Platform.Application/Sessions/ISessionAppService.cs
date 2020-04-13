using System.Threading.Tasks;
using Abp.Application.Services;
using NextGen-Auction-Platform.Sessions.Dto;

namespace NextGen-Auction-Platform.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
