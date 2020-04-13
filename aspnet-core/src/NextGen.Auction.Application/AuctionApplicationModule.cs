using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using NextGen.Auction.Authorization;

namespace NextGen.Auction
{
    [DependsOn(
        typeof(AuctionCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class AuctionApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<AuctionAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(AuctionApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
