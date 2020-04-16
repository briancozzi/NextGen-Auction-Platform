using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.Security
{
    public interface IPasswordComplexitySettingStore
    {
        Task<PasswordComplexitySetting> GetSettingsAsync();
    }
}
