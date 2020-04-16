using System.Threading.Tasks;
using Abp.Application.Services;
using NextGen.BiddingPlatform.Sessions.Dto;

namespace NextGen.BiddingPlatform.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
