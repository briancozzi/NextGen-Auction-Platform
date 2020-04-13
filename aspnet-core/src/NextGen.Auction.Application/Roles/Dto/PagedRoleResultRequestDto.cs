using Abp.Application.Services.Dto;

namespace NextGen.Auction.Roles.Dto
{
    public class PagedRoleResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}

