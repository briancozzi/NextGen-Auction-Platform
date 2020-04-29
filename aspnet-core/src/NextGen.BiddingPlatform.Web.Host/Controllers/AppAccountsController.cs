using Abp.AspNetCore.Mvc.Authorization;
using NextGen.BiddingPlatform.AppAccount;
using NextGen.BiddingPlatform.Storage;

namespace NextGen.BiddingPlatform.Web.Controllers
{
    public class AppAccountsController : AppAccountsControllerBase
    {
        public AppAccountsController(IAppAccountAppService appAccountAppService):base(appAccountAppService)
        {
        }
    }
}