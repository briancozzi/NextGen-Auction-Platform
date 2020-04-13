using Abp.Application.Services.Dto;

namespace NextGen-Auction-Platform.Roles.Dto
{
    public class PagedRoleResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}

