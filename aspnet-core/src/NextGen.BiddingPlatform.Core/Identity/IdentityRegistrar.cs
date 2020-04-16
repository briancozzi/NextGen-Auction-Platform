using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NextGen.BiddingPlatform.Authentication.TwoFactor.Google;
using NextGen.BiddingPlatform.Authorization;
using NextGen.BiddingPlatform.Authorization.Roles;
using NextGen.BiddingPlatform.Authorization.Users;
using NextGen.BiddingPlatform.Editions;
using NextGen.BiddingPlatform.MultiTenancy;

namespace NextGen.BiddingPlatform.Identity
{
    public static class IdentityRegistrar
    {
        public static IdentityBuilder Register(IServiceCollection services)
        {
            services.AddLogging();

            return services.AddAbpIdentity<Tenant, User, Role>(options =>
                {
                    options.Tokens.ProviderMap[GoogleAuthenticatorProvider.Name] = new TokenProviderDescriptor(typeof(GoogleAuthenticatorProvider));
                })
                .AddAbpTenantManager<TenantManager>()
                .AddAbpUserManager<UserManager>()
                .AddAbpRoleManager<RoleManager>()
                .AddAbpEditionManager<EditionManager>()
                .AddAbpUserStore<UserStore>()
                .AddAbpRoleStore<RoleStore>()
                .AddAbpSignInManager<SignInManager>()
                .AddAbpUserClaimsPrincipalFactory<UserClaimsPrincipalFactory>()
                .AddAbpSecurityStampValidator<SecurityStampValidator>()
                .AddPermissionChecker<PermissionChecker>()
                .AddDefaultTokenProviders();
        }
    }
}
