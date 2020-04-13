using System.Threading.Tasks;
using NextGen.Auction.Configuration.Dto;

namespace NextGen.Auction.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
