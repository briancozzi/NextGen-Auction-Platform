using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using NextGen.BiddingPlatform.Authorization.Users;
using NextGen.BiddingPlatform.MultiTenancy;

namespace NextGen.BiddingPlatform.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}