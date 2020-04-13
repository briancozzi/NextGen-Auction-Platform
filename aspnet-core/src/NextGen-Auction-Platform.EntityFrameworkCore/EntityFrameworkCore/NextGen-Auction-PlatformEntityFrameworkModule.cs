using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using NextGen-Auction-Platform.EntityFrameworkCore.Seed;

namespace NextGen-Auction-Platform.EntityFrameworkCore
{
    [DependsOn(
        typeof(NextGen-Auction-PlatformCoreModule), 
        typeof(AbpZeroCoreEntityFrameworkCoreModule))]
    public class NextGen-Auction-PlatformEntityFrameworkModule : AbpModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.AbpEfCore().AddDbContext<NextGen-Auction-PlatformDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        NextGen-Auction-PlatformDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        NextGen-Auction-PlatformDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                    }
                });
            }
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(NextGen-Auction-PlatformEntityFrameworkModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!SkipDbSeed)
            {
                SeedHelper.SeedHostDb(IocManager);
            }
        }
    }
}
