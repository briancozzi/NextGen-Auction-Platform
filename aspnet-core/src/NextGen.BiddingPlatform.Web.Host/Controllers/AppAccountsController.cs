using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Hosting;
using NextGen.BiddingPlatform.AppAccount;
using NextGen.BiddingPlatform.Storage;

namespace NextGen.BiddingPlatform.Web.Controllers
{
    public class AppAccountsController : AppAccountsControllerBase
    {
        public AppAccountsController(IWebHostEnvironment webHostEnvironment, IAppAccountAppService appAccountAppService):base(webHostEnvironment, appAccountAppService)
        {
        }
    }
}