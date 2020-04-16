using Abp.Dependency;
using NextGen.BiddingPlatform.Configuration;
using NextGen.BiddingPlatform.Url;
using NextGen.BiddingPlatform.Web.Url;

namespace NextGen.BiddingPlatform.Web.Public.Url
{
    public class WebUrlService : WebUrlServiceBase, IWebUrlService, ITransientDependency
    {
        public WebUrlService(
            IAppConfigurationAccessor appConfigurationAccessor) :
            base(appConfigurationAccessor)
        {
        }

        public override string WebSiteRootAddressFormatKey => "App:WebSiteRootAddress";

        public override string ServerRootAddressFormatKey => "App:AdminWebSiteRootAddress";
    }
}