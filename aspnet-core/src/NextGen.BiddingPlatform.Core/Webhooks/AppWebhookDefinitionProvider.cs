using Abp.Localization;
using Abp.Webhooks;

namespace NextGen.BiddingPlatform.WebHooks
{
    public class AppWebhookDefinitionProvider : WebhookDefinitionProvider
    {
        public override void SetWebhooks(IWebhookDefinitionContext context)
        {
            context.Manager.Add(new WebhookDefinition(
                name: AppWebHookNames.TestWebhook
            ));

            //Add your webhook definitions here 
            context.Manager.Add(new WebhookDefinition(
                name: AppWebHookNames.TestAuctionHistoryWebhook
            ));

            context.Manager.Add(new WebhookDefinition(
               name: AppWebHookNames.CloseBiddingOnEventOrItem
           ));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, BiddingPlatformConsts.LocalizationSourceName);
        }
    }
}
