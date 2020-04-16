using Abp.Authorization;
using NextGen.BiddingPlatform.Authorization.Roles;
using NextGen.BiddingPlatform.Authorization.Users;

namespace NextGen.BiddingPlatform.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
