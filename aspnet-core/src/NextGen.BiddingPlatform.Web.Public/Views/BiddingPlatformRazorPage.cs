using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace NextGen.BiddingPlatform.Web.Public.Views
{
    public abstract class BiddingPlatformRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected BiddingPlatformRazorPage()
        {
            LocalizationSourceName = BiddingPlatformConsts.LocalizationSourceName;
        }
    }
}
