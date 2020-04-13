using System.Threading.Tasks;
using NextGen-Auction-Platform.Configuration.Dto;

namespace NextGen-Auction-Platform.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
