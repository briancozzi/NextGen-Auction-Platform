using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using NextGen.Auction.Configuration.Dto;

namespace NextGen.Auction.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : AuctionAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
