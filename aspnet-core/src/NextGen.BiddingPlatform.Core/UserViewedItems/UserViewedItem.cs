using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using NextGen.BiddingPlatform.Authorization.Users;
using NextGen.BiddingPlatform.Core.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NextGen.BiddingPlatform.UserViewedItems
{
    [Table("UserViewedItems")]
    public class UserViewedItem : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }
        public int ItemId { get; set; }
        [ForeignKey("ItemId")]
        public Item ItemFk { get; set; }
    }
}
