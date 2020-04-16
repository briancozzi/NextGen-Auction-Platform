using Abp.AspNetCore.Mvc.Views;

namespace NextGen.BiddingPlatform.Web.Views
{
    public abstract class BiddingPlatformRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected BiddingPlatformRazorPage()
        {
            LocalizationSourceName = BiddingPlatformConsts.LocalizationSourceName;
        }
    }
}
