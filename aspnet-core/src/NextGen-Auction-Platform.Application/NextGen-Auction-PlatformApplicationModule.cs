using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using NextGen-Auction-Platform.Authorization;

namespace NextGen-Auction-Platform
{
    [DependsOn(
        typeof(NextGen-Auction-PlatformCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class NextGen-Auction-PlatformApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<NextGen-Auction-PlatformAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(NextGen-Auction-PlatformApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
