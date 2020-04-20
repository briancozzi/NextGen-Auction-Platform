using Abp.Authorization.Users;
using Abp.Domain.Entities.Auditing;
using NextGen.BiddingPlatform.CustomInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema;

namespace NextGen.BiddingPlatform.AddressEntity
{
    [Table("Addresses")]
    public class Address : AuditedEntity, IHasUniqueIdentifier
    {
        [Index("IX_Address", 1, IsUnique = true, IsClustered = false)]
        public Guid UniqueId { get; set; }

        [Required]
        [MaxLength(AbpUserBase.MaxUserNameLength)]
        public string Address1 { get; set; }

        [MaxLength(AbpUserBase.MaxUserNameLength)]
        public string Address2 { get; set; }

        [Required]
        [MaxLength(25)]
        public string City { get; set; }

        [ForeignKey("State")]
        public int StateId { get; set; }
        public State State { get; set; }

        [Required]
        [MaxLength(5)]
        public string ZipCode { get; set; }
    }
}
