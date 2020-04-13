using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Timing;
using Abp.Zero;
using Abp.Zero.Configuration;
using NextGen-Auction-Platform.Authorization.Roles;
using NextGen-Auction-Platform.Authorization.Users;
using NextGen-Auction-Platform.Configuration;
using NextGen-Auction-Platform.Localization;
using NextGen-Auction-Platform.MultiTenancy;
using NextGen-Auction-Platform.Timing;

namespace NextGen-Auction-Platform
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class NextGen-Auction-PlatformCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            // Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            NextGen-Auction-PlatformLocalizationConfigurer.Configure(Configuration.Localization);

            // Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = NextGen-Auction-PlatformConsts.MultiTenancyEnabled;

            // Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            Configuration.Settings.Providers.Add<AppSettingProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(NextGen-Auction-PlatformCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}
