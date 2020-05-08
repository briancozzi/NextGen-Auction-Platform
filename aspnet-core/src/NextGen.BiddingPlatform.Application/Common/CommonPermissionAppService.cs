using Abp.Application.Services.Dto;
using Abp.Authorization;
using NextGen.BiddingPlatform.Authorization.Users;
using NextGen.BiddingPlatform.Common.Dto;
using NextGen.BiddingPlatform.CustomAuthorization;
using System;
using System.Collections.Generic;
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
            var permissions = await _userService.GetUserPermissionsForEdit(new EntityDto<long>(userId));
            return permissions.GrantedPermissionNames;
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
