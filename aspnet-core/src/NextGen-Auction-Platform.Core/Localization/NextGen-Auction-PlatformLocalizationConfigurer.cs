using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace NextGen-Auction-Platform.Localization
{
    public static class NextGen-Auction-PlatformLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(NextGen-Auction-PlatformConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(NextGen-Auction-PlatformLocalizationConfigurer).GetAssembly(),
                        "NextGen-Auction-Platform.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
