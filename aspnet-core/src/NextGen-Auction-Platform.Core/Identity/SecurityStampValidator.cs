using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Abp.Authorization;
using NextGen-Auction-Platform.Authorization.Roles;
using NextGen-Auction-Platform.Authorization.Users;
using NextGen-Auction-Platform.MultiTenancy;
using Microsoft.Extensions.Logging;

namespace NextGen-Auction-Platform.Identity
{
    public class SecurityStampValidator : AbpSecurityStampValidator<Tenant, Role, User>
    {
        public SecurityStampValidator(
            IOptions<SecurityStampValidatorOptions> options,
            SignInManager signInManager,
            ISystemClock systemClock,
            ILoggerFactory loggerFactory) 
            : base(options, signInManager, systemClock, loggerFactory)
        {
        }
    }
}
