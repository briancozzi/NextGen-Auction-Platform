using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace NextGen.BiddingPlatform
{
    [DependsOn(typeof(BiddingPlatformClientModule), typeof(AbpAutoMapperModule))]
    public class BiddingPlatformXamarinSharedModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BiddingPlatformXamarinSharedModule).GetAssembly());
        }
    }
}