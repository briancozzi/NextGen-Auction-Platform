using Abp.Events.Bus;

namespace NextGen.BiddingPlatform.MultiTenancy
{
    public class RecurringPaymentsEnabledEventData : EventData
    {
        public int TenantId { get; set; }
    }
}