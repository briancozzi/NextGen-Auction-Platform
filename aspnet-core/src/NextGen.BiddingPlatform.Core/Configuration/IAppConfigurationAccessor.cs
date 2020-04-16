using Microsoft.Extensions.Configuration;

namespace NextGen.BiddingPlatform.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
