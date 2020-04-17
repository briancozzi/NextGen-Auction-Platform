using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using NextGen.BiddingPlatform.AddressEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NextGen.BiddingPlatform.Events
{
    [Table("Events")]
    public class Event : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [ForeignKey("AppAccount")]
        public Guid AppAccountId { get; set; }

        [Required]
        [MaxLength(AbpUserBase.MaxUserNameLength)]
        public string EventName { get; set; }

        public DateTime EventDate { get; set; }

        public DateTime EventStartDateTime { get; set; }
        public DateTime EventEndDateTime { get; set; }

        [ForeignKey("Address")]
        public Guid AddressId { get; set; }
        public Address Address { get; set; }
    }
}
