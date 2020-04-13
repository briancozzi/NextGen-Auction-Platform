using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using NextGen.Auction.Configuration;

namespace NextGen.Auction.Web.Host.Startup
{
    [DependsOn(
       typeof(AuctionWebCoreModule))]
    public class AuctionWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public AuctionWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AuctionWebHostModule).GetAssembly());
        }
    }
}
