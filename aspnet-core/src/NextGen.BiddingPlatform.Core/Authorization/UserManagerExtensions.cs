using System.Threading.Tasks;
using Abp.Authorization.Users;
using NextGen.BiddingPlatform.Authorization.Users;

namespace NextGen.BiddingPlatform.Authorization
{
    public static class UserManagerExtensions
    {
        public static async Task<User> GetAdminAsync(this UserManager userManager)
        {
            return await userManager.FindByNameAsync(AbpUserBase.AdminUserName);
        }
    }
}
