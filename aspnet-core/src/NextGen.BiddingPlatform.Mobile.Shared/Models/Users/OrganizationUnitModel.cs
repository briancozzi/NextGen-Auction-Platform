using Abp.AutoMapper;
using NextGen.BiddingPlatform.Organizations.Dto;

namespace NextGen.BiddingPlatform.Models.Users
{
    [AutoMapFrom(typeof(OrganizationUnitDto))]
    public class OrganizationUnitModel : OrganizationUnitDto
    {
        public bool IsAssigned { get; set; }
    }
}