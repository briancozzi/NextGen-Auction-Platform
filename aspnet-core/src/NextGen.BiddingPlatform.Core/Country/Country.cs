using Abp.Authorization.Users;
using Abp.Domain.Entities.Auditing;
using NextGen.BiddingPlatform.CustomInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema;

namespace NextGen.BiddingPlatform.Country
{
    [Table("Countries")]
    public class Country : AuditedEntity, IHasUniqueIdentifier
    {
        public const int MaxCountryCodeLength = 3;

        [Index("IX_Country", IsClustered = false, IsUnique = true)]
        public Guid UniqueId { get; set; }

        [Required]
        [MaxLength(MaxCountryCodeLength)]
        public string CountryCode { get; set; }

        [Required]
        [MaxLength(AbpUserBase.MaxUserNameLength)]
        public string CountryName { get; set; }
    }
}
