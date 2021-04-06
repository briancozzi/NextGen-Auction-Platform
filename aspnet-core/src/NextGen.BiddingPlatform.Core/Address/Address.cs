using Abp.Authorization.Users;
using Abp.Domain.Entities.Auditing;
using NextGen.BiddingPlatform.CustomInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema;

namespace NextGen.BiddingPlatform.Core.Addresses
{
    public class Address : AuditedEntity, IHasUniqueIdentifier
    {
        public const int MaxCityLength = 25;

        [Index("IX_Address_UniqueId", IsClustered = false, IsUnique = true)]
        public Guid UniqueId { get; set; }

        [Required]
        [MaxLength(AbpUserBase.MaxUserNameLength)]
        public string Address1 { get; set; }

        [MaxLength(AbpUserBase.MaxUserNameLength)]
        public string Address2 { get; set; }

        [Required]
        [MaxLength(MaxCityLength)]
        public string City { get; set; }

        [ForeignKey("State")]
        public int StateId { get; set; }
        public State.State State { get; set; }

        [ForeignKey("Country")]
        public int CountryId { get; set; }
        public Country.Country Country { get; set; }

        [Required]
        public string ZipCode { get; set; }
    }
}
