using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using NextGen.BiddingPlatform.Core.Auctions;
using NextGen.BiddingPlatform.Core.Items;
using NextGen.BiddingPlatform.CustomInterface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema;

namespace NextGen.BiddingPlatform.Core.AuctionItems
{
    [Table("AuctionItems")]
    public class AuctionItem : AuditedEntity, IHasUniqueIdentifier, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Index("IX_AuctionItem_UniqueId", IsClustered = false, IsUnique = true)]
        public Guid UniqueId { get; set; }

        [ForeignKey("Auction")]
        public int AuctionId { get; set; }
        public Auction Auction { get; set; }

        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public Item Item { get; set; }

        public bool IsActive { get; set; }
        public bool IsBiddingClosed { get; set; }

        public string PaymentStatus { get; set; }
        public DateTime? PaymentStatusUpdateDate { get; set; }

        public ICollection<AuctionHistories.AuctionHistory> AuctionHistories { get; set; }
        public AuctionItem()
        {
            AuctionHistories = new Collection<AuctionHistories.AuctionHistory>();
        }
    }
}
