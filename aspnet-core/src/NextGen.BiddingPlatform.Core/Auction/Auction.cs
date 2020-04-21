using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.VisualBasic;
using NextGen.BiddingPlatform.Core.Addresses;
using NextGen.BiddingPlatform.Core.AppAccountEvents;
using NextGen.BiddingPlatform.Core.AppAccounts;
using NextGen.BiddingPlatform.Core.AuctionItems;
using NextGen.BiddingPlatform.CustomInterface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema;

namespace NextGen.BiddingPlatform.Core.Auctions
{
    [Table("Auctions")]
    public class Auction : AuditedEntity,IHasUniqueIdentifier,IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Index("IX_Auction_UniqueId", IsClustered = false, IsUnique = true)]
        public Guid UniqueId { get; set; }

        [ForeignKey("AppAccount")]
        public int AppAccountId { get; set; }
        public AppAccount AppAccount { get; set; }

        [ForeignKey("Event")]
        public int EventId { get; set; }
        public Event Event { get; set; }

        [Required]
        public DateTime AuctionStartDateTime { get; set; }

        [Required]
        public DateTime AuctionEndDateTime { get; set; }

        [ForeignKey("Address")]
        public int AddressId { get; set; }
        public Address Address { get; set; }

        public virtual ICollection<AuctionItem> AuctionItems { get; set; }

        public Auction()
        {
            AuctionItems = new Collection<AuctionItem>();
        }
    }
}
