using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using NextGen.BiddingPlatform.CustomInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema;

namespace NextGen.BiddingPlatform.State
{
    [Table("States")]
    public class State : CreationAuditedEntity, IHasUniqueIdentifier
    {
        public const int MaxStateNameLength = 25;
        public const int MaxStateCodeLength = 3;

        [Index("IX_State", IsClustered = false, IsUnique = true)]
        public Guid UniqueId { get; set; }
        [Required]
        [MaxLength(MaxStateNameLength)]
        public string StateName { get; set; }

        [Required]
        [MaxLength(MaxStateCodeLength)]
        public string StateCode { get; set; }

        [ForeignKey("Country")]
        public int CountryId { get; set; }
        public Country.Country Country { get; set; }
    }
}
