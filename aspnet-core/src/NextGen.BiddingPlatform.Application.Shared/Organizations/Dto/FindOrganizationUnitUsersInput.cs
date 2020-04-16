using NextGen.BiddingPlatform.Dto;

namespace NextGen.BiddingPlatform.Organizations.Dto
{
    public class FindOrganizationUnitUsersInput : PagedAndFilteredInputDto
    {
        public long OrganizationUnitId { get; set; }
    }
}
