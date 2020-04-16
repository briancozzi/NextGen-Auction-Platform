using System.Threading.Tasks;
using Abp.Application.Services;
using NextGen.BiddingPlatform.Configuration.Tenants.Dto;

namespace NextGen.BiddingPlatform.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}
