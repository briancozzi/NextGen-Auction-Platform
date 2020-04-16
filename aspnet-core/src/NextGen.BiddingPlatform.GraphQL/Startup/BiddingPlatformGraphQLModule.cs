using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace NextGen.BiddingPlatform.Startup
{
    [DependsOn(typeof(BiddingPlatformCoreModule))]
    public class BiddingPlatformGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BiddingPlatformGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}