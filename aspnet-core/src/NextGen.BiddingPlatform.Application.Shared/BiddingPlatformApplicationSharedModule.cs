using Abp.Modules;
using Abp.Reflection.Extensions;

namespace NextGen.BiddingPlatform
{
    [DependsOn(typeof(BiddingPlatformCoreSharedModule))]
    public class BiddingPlatformApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BiddingPlatformApplicationSharedModule).GetAssembly());
        }
    }
}