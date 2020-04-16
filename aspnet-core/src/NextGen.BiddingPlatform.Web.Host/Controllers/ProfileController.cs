using Abp.AspNetCore.Mvc.Authorization;
using NextGen.BiddingPlatform.Storage;

namespace NextGen.BiddingPlatform.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ProfileController : ProfileControllerBase
    {
        public ProfileController(ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
        }
    }
}