using Abp.Dependency;
using Abp.Reflection.Extensions;
using Microsoft.Extensions.Configuration;
using NextGen.BiddingPlatform.Configuration;

namespace NextGen.BiddingPlatform.Test.Base.Configuration
{
    public class TestAppConfigurationAccessor : IAppConfigurationAccessor, ISingletonDependency
    {
        public IConfigurationRoot Configuration { get; }

        public TestAppConfigurationAccessor()
        {
            Configuration = AppConfigurations.Get(
                typeof(BiddingPlatformTestBaseModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }
    }
}
