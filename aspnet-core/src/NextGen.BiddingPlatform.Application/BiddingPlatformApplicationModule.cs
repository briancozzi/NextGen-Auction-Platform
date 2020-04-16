using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using NextGen.BiddingPlatform.Authorization;

namespace NextGen.BiddingPlatform
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(BiddingPlatformApplicationSharedModule),
        typeof(BiddingPlatformCoreModule)
        )]
    public class BiddingPlatformApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BiddingPlatformApplicationModule).GetAssembly());
        }
    }
}