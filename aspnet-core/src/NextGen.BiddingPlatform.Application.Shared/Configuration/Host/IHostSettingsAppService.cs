using System.Threading.Tasks;
using Abp.Application.Services;
using NextGen.BiddingPlatform.Configuration.Host.Dto;

namespace NextGen.BiddingPlatform.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
