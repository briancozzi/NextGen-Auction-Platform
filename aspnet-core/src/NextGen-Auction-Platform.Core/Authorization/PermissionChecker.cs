using Abp.Authorization;
using NextGen-Auction-Platform.Authorization.Roles;
using NextGen-Auction-Platform.Authorization.Users;

namespace NextGen-Auction-Platform.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
