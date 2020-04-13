using System.Threading.Tasks;
using Abp.Application.Services;
using NextGen.Auction.Sessions.Dto;

namespace NextGen.Auction.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
