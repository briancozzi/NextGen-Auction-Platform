using Abp.Modules;
using Abp.Reflection.Extensions;

namespace NextGen.BiddingPlatform
{
    [DependsOn(typeof(BiddingPlatformXamarinSharedModule))]
    public class BiddingPlatformXamarinIosModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BiddingPlatformXamarinIosModule).GetAssembly());
        }
    }
}