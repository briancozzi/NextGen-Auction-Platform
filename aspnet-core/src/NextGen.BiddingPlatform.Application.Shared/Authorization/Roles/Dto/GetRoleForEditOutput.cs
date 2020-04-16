using System.Collections.Generic;
using NextGen.BiddingPlatform.Authorization.Permissions.Dto;

namespace NextGen.BiddingPlatform.Authorization.Roles.Dto
{
    public class GetRoleForEditOutput
    {
        public RoleEditDto Role { get; set; }

        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}