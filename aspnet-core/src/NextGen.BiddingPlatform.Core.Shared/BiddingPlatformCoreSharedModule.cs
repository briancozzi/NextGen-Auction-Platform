using Abp.Modules;
using Abp.Reflection.Extensions;

namespace NextGen.BiddingPlatform
{
    public class BiddingPlatformCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BiddingPlatformCoreSharedModule).GetAssembly());
        }
    }
}