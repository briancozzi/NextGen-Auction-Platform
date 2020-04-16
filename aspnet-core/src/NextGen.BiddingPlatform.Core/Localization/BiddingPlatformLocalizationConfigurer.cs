using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace NextGen.BiddingPlatform.Localization
{
    public static class BiddingPlatformLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    BiddingPlatformConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(BiddingPlatformLocalizationConfigurer).GetAssembly(),
                        "NextGen.BiddingPlatform.Localization.BiddingPlatform"
                    )
                )
            );
        }
    }
}