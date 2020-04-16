using System.Threading.Tasks;
using NextGen.BiddingPlatform.ApiClient.Models;

namespace NextGen.BiddingPlatform.Services.Account
{
    public interface IAccountService
    {
        AbpAuthenticateModel AbpAuthenticateModel { get; set; }
        
        AbpAuthenticateResultModel AuthenticateResultModel { get; set; }
        
        Task LoginUserAsync();

        Task LogoutAsync();
    }
}
