using Abp.Application.Services.Dto;

namespace NextGen.BiddingPlatform.ApplicationConfigurations.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}