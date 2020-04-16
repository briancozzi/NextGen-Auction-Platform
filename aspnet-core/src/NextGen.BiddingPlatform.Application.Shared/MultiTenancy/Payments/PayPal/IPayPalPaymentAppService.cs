using System.Threading.Tasks;
using Abp.Application.Services;
using NextGen.BiddingPlatform.MultiTenancy.Payments.PayPal.Dto;

namespace NextGen.BiddingPlatform.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}
