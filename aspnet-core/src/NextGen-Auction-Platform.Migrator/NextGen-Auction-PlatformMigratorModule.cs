using Microsoft.Extensions.Configuration;
using Castle.MicroKernel.Registration;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using NextGen-Auction-Platform.Configuration;
using NextGen-Auction-Platform.EntityFrameworkCore;
using NextGen-Auction-Platform.Migrator.DependencyInjection;

namespace NextGen-Auction-Platform.Migrator
{
    [DependsOn(typeof(NextGen-Auction-PlatformEntityFrameworkModule))]
    public class NextGen-Auction-PlatformMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public NextGen-Auction-PlatformMigratorModule(NextGen-Auction-PlatformEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(NextGen-Auction-PlatformMigratorModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                NextGen-Auction-PlatformConsts.ConnectionStringName
            );

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(
                typeof(IEventBus), 
                () => IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                )
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(NextGen-Auction-PlatformMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}
