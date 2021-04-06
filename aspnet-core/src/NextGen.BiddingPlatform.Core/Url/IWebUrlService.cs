using System.Collections.Generic;

namespace NextGen.BiddingPlatform.Url
{
    public interface IWebUrlService
    {
        string WebSiteRootAddressFormat { get; }

        string ServerRootAddressFormat { get; }

        bool SupportsTenancyNameInUrl { get; }

        string GetSiteRootAddress(string tenancyName = null);

        string GetServerRootAddress(string tenancyName = null);

        string GetApiServerRootAddress(string tenancyName = null);

        List<string> GetRedirectAllowedExternalWebSites();
    }
}
