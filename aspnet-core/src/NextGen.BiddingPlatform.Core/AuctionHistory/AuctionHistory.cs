using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using NextGen.BiddingPlatform.Core.AuctionBidders;
using NextGen.BiddingPlatform.Core.AuctionItems;
using NextGen.BiddingPlatform.CustomInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema;

namespace NextGen.BiddingPlatform.Core.AuctionHistories
{
    [Table("AuctionHistory")]
    public class AuctionHistory : FullAuditedEntity, IHasUniqueIdentifier
    {
        [Index("IX_AuctionHistory_UniqueId", IsClustered = false, IsUnique = true)]
        public Guid UniqueId { get; set; }

        [ForeignKey("AuctionBidder")]
        public int AuctionBidderId { get; set; }
        public AuctionBidder AuctionBidder { get; set; }

        [ForeignKey("AuctionItem")]
        public int AuctionItemId { get; set; }
        public AuctionItem AuctionItem { get; set; }

        [Required]
        public double BidAmount { get; set; }

        public string BidStatus { get; set; }//like winning, pending
    }
}
