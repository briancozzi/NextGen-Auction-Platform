using NextGen.BiddingPlatform.Dto;

namespace NextGen.BiddingPlatform.Organizations.Dto
{
    public class FindOrganizationUnitRolesInput : PagedAndFilteredInputDto
    {
        public long OrganizationUnitId { get; set; }
    }
}