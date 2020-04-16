using System.Threading.Tasks;
using Abp.Application.Services;
using NextGen.BiddingPlatform.Install.Dto;

namespace NextGen.BiddingPlatform.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}