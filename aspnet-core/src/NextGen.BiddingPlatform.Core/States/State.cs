using Abp.Domain.Entities;
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
    [Table("States")]
    public class State :  CreationAuditedEntity, IHasUniqueIdentifier
    {
        [Index("IX_State", 1, IsUnique = true, IsClustered = false)]
        public Guid UniqueId { get; set; }
        [Required]
        [MaxLength(25)]
        public string StateName { get; set; }

        [Required]
        [MaxLength(3)]
        public string StateCode { get; set; }

        [ForeignKey("Country")]
        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}
