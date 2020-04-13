using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using NextGen-Auction-Platform.Configuration.Dto;

namespace NextGen-Auction-Platform.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : NextGen-Auction-PlatformAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
