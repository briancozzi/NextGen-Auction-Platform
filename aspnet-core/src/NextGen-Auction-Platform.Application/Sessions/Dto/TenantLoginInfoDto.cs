using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using NextGen-Auction-Platform.MultiTenancy;

namespace NextGen-Auction-Platform.Sessions.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}
