using Abp.AspNetCore.Mvc.Authorization;
using NextGen.BiddingPlatform.Authorization;
using NextGen.BiddingPlatform.Storage;
using Abp.BackgroundJobs;

namespace NextGen.BiddingPlatform.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UsersController : UsersControllerBase
    {
        public UsersController(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}