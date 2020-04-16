using Abp.Modules;
using Abp.Reflection.Extensions;

namespace NextGen.BiddingPlatform
{
    public class BiddingPlatformClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BiddingPlatformClientModule).GetAssembly());
        }
    }
}
