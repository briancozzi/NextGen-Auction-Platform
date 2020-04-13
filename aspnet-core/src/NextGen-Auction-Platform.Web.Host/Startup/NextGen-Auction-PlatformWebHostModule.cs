using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using NextGen-Auction-Platform.Configuration;

namespace NextGen-Auction-Platform.Web.Host.Startup
{
    [DependsOn(
       typeof(NextGen-Auction-PlatformWebCoreModule))]
    public class NextGen-Auction-PlatformWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public NextGen-Auction-PlatformWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(NextGen-Auction-PlatformWebHostModule).GetAssembly());
        }
    }
}
