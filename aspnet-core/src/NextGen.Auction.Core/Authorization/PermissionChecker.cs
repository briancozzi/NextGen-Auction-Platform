using Abp.Authorization;
using NextGen.Auction.Authorization.Roles;
using NextGen.Auction.Authorization.Users;

namespace NextGen.Auction.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
