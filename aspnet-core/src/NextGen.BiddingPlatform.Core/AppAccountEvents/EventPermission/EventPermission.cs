using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using NextGen.BiddingPlatform.Authorization.Users;
using NextGen.BiddingPlatform.Core.AppAccountEvents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NextGen.BiddingPlatform.AppAccountEvents.EventPermission
{
    public class EventPermission : AuditedEntity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [ForeignKey("User")]
        public long UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Event")]
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
