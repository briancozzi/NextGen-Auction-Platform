using System.Globalization;

namespace NextGen.BiddingPlatform.Localization
{
    public interface IApplicationCulturesProvider
    {
        CultureInfo[] GetAllCultures();
    }
}