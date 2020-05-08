using Abp.Application.Services.Dto;
using Abp.Authorization;
using NextGen.BiddingPlatform.Authorization.Permissions.Dto;
using NextGen.BiddingPlatform.Authorization.Users;
using NextGen.BiddingPlatform.Authorization.Users.Dto;
using NextGen.BiddingPlatform.Common.Dto;
using NextGen.BiddingPlatform.CustomAuthorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.Common
{
    [AbpAuthorize]
    public class CommonPermissionAppService : BiddingPlatformAppServiceBase, ICommonPermissionAppService
    {
        private readonly IUserAppService _userService;
        public CommonPermissionAppService(IUserAppService userService)
        {
            _userService = userService;
        }
        
        public async Task<List<string>> GetUserPermissions()
        {
            var userId = AbpSession.UserId.Value;
            var user = await UserManager.GetUserByIdAsync(userId);
            var grantedPermissions = await UserManager.GetGrantedPermissionsAsync(user);

            return grantedPermissions.Select(p=>p.Name).ToList();
        }

        public async Task<PermissionDto> HasPermission(string assignPermission, string allPermission)
        {
            var userPermission = await GetUserPermissions();

            return new PermissionDto
            {
                HasAllPermission = userPermission.Contains(allPermission),
                HasAssignPermission = userPermission.Contains(assignPermission)
            };
        }
    }
}
