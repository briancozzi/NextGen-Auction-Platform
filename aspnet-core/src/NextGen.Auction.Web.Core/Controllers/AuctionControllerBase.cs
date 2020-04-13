using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace NextGen.Auction.Controllers
{
    public abstract class AuctionControllerBase: AbpController
    {
        protected AuctionControllerBase()
        {
            LocalizationSourceName = AuctionConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
