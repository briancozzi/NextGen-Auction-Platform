using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using NextGen.BiddingPlatform.Configuration.Dto;
using NextGen.BiddingPlatform.Configuration.Host.Dto;

namespace NextGen.BiddingPlatform.Install.Dto
{
    public class InstallDto
    {
        [Required]
        [DisableAuditing]
        public string ConnectionString { get; set; }

        [Required]
        [DisableAuditing]
        public string AdminPassword { get; set; }

        [Required]
        public string WebSiteUrl { get; set; }

        public string ServerUrl { get; set; }

        [Required]
        public string DefaultLanguage { get; set; }

        public EmailSettingsEditDto SmtpSettings { get; set; }

        public HostBillingSettingsEditDto BillInfo { get; set; }
    }
}