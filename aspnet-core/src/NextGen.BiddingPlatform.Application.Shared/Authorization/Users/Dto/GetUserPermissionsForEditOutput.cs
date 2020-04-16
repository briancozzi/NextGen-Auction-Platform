using System.Collections.Generic;
using NextGen.BiddingPlatform.Authorization.Permissions.Dto;

namespace NextGen.BiddingPlatform.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}