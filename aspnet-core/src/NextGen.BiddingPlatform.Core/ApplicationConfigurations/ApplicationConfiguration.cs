using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace NextGen.BiddingPlatform.ApplicationConfigurations
{
    [Table("ApplicationConfigurations")]
    public class ApplicationConfiguration : FullAuditedEntity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Required]
        public virtual string ConfigKey { get; set; }

        [Required]
        public virtual string ConfigValue { get; set; }

    }
}