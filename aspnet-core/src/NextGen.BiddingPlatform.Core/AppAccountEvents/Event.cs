using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using NextGen.BiddingPlatform.Core.Addresses;
using NextGen.BiddingPlatform.Core.AppAccounts;
using NextGen.BiddingPlatform.Core.Auctions;
using NextGen.BiddingPlatform.CustomInterface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Toolbelt.ComponentModel.DataAnnotations.Schema;

namespace NextGen.BiddingPlatform.Core.AppAccountEvents
{
    [Table("AppAccountEvents")]
    public class Event : FullAuditedEntity, IMustHaveTenant, IHasUniqueIdentifier
    {
        public int TenantId { get; set; }

        [Index("IX_AppAccountEvent_UniqueId", IsClustered = false, IsUnique = true)]
        public Guid UniqueId { get; set; }

        [ForeignKey("AppAccount")]
        public int AppAccountId { get; set; }
        public AppAccount AppAccount { get; set; }

        [Required]
        [MaxLength(AbpUserBase.MaxUserNameLength)]
        public string EventName { get; set; }

        [Required]
        public DateTime EventStartDateTime { get; set; }

        [Required]
        public DateTime EventEndDateTime { get; set; }

        public string Email { get; set; }

        public string MobileNo { get; set; }

        [ForeignKey("Address")]
        public int AddressId { get; set; }
        public Address Address { get; set; }

        public string EventUrl { get; set; }
        public string TimeZone { get; set; }// may be we have timezone table for this field
        public bool IsActive { get; set; }
        public ICollection<Auction> EventAuctions { get; set; }
        public Event()
        {
            EventAuctions = new Collection<Auction>();
            Address = new Address();
        }
    }
}
