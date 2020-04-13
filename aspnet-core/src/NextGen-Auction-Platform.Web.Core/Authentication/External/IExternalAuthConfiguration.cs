using System.Collections.Generic;

namespace NextGen-Auction-Platform.Authentication.External
{
    public interface IExternalAuthConfiguration
    {
        List<ExternalLoginProviderInfo> Providers { get; }
    }
}
