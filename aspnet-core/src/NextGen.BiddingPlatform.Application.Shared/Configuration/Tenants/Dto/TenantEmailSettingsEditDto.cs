using Abp.Auditing;
using NextGen.BiddingPlatform.Configuration.Dto;

namespace NextGen.BiddingPlatform.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}