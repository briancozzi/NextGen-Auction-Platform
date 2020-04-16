using System.Threading.Tasks;
using NextGen.BiddingPlatform.Sessions.Dto;

namespace NextGen.BiddingPlatform.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
