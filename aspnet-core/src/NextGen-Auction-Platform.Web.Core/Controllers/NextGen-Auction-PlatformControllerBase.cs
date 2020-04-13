using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace NextGen-Auction-Platform.Controllers
{
    public abstract class NextGen-Auction-PlatformControllerBase: AbpController
    {
        protected NextGen-Auction-PlatformControllerBase()
        {
            LocalizationSourceName = NextGen-Auction-PlatformConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
