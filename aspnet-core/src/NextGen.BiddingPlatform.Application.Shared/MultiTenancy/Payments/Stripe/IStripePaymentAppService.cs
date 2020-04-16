using System.Threading.Tasks;
using Abp.Application.Services;
using NextGen.BiddingPlatform.MultiTenancy.Payments.Dto;
using NextGen.BiddingPlatform.MultiTenancy.Payments.Stripe.Dto;

namespace NextGen.BiddingPlatform.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();

        Task<SubscriptionPaymentDto> GetPaymentAsync(StripeGetPaymentInput input);

        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}