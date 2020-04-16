using Abp.AspNetCore.Mvc.ViewComponents;

namespace NextGen.BiddingPlatform.Web.Public.Views
{
    public abstract class BiddingPlatformViewComponent : AbpViewComponent
    {
        protected BiddingPlatformViewComponent()
        {
            LocalizationSourceName = BiddingPlatformConsts.LocalizationSourceName;
        }
    }
}