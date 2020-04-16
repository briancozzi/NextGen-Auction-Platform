using NextGen.BiddingPlatform.Dto;

namespace NextGen.BiddingPlatform.WebHooks.Dto
{
    public class GetAllSendAttemptsInput : PagedInputDto
    {
        public string SubscriptionId { get; set; }
    }
}
