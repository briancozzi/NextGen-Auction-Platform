using Abp.Application.Services;
using Abp.Application.Services.Dto;
using NextGen.BiddingPlatform.Authorization.Permissions.Dto;

namespace NextGen.BiddingPlatform.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
