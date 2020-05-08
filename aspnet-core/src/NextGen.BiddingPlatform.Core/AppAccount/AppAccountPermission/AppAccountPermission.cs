using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using NextGen.BiddingPlatform.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NextGen.BiddingPlatform.AppAccount.AppAccountPermission
{
    public class AppAccountPermission : AuditedEntity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [ForeignKey("AppAccount")]
        public int AppAccountId { get; set; }
        public Core.AppAccounts.AppAccount AppAccount { get; set; }

        [ForeignKey("User")]
        public long UserId { get; set; }
        public User User { get; set; }

        public string PermissionName { get; set; }
    }
}
