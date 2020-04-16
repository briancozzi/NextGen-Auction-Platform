using System.Collections.Generic;

namespace NextGen.BiddingPlatform.MultiTenancy.Payments
{
    public interface IPaymentGatewayStore
    {
        List<PaymentGatewayModel> GetActiveGateways();
    }
}
