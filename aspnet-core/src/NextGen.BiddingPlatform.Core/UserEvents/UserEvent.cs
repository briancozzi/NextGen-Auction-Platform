using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using NextGen.BiddingPlatform.Authorization.Users;
using NextGen.BiddingPlatform.Core.AppAccountEvents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NextGen.BiddingPlatform.UserEvents
{
    [Table("UserEvents")]
    public class UserEvent :  FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public long UserId { get; set; }
        public User UserFk { get; set; }

        public int EventId { get; set; }
        public Event EventFk { get; set; }
    }
}
