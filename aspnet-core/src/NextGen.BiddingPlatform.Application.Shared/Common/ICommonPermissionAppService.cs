using Abp.Application.Services;
using NextGen.BiddingPlatform.Common.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.Common
{
    public interface ICommonPermissionAppService : IApplicationService
    {
        Task<List<string>> GetUserPermissions();
        Task<PermissionDto> HasPermission(string assignPermission, string allPermission);
    }
}
