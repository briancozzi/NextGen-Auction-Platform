using Abp.MultiTenancy;
using NextGen-Auction-Platform.Authorization.Users;

namespace NextGen-Auction-Platform.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}
