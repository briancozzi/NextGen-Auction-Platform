using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NextGen.BiddingPlatform.AddressEntity
{
    [Table("States")]
    public class State :  CreationAuditedEntity<Guid>
    {
        [Required]
        [MaxLength(25)]
        public string StateName { get; set; }

        [Required]
        [MaxLength(3)]
        public string StateCode { get; set; }
    }
}
