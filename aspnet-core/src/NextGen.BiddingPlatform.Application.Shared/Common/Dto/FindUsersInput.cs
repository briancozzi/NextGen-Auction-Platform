using NextGen.BiddingPlatform.Dto;

namespace NextGen.BiddingPlatform.Common.Dto
{
    public class FindUsersInput : PagedAndFilteredInputDto
    {
        public int? TenantId { get; set; }
    }
}