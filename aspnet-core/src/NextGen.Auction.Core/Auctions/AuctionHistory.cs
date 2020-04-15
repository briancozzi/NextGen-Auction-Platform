using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NextGen.Auction.Auctions
{
    [Table("AuctionHistory")]
    public class AuctionHistory : AuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        [ForeignKey("AuctionBidder")]
        public Guid AuctionBidderId { get; set; }
        public AuctionBidder AuctionBidder { get; set; }
        [ForeignKey("AuctionItem")]
        public Guid AuctionItemId { get; set; }
        public AuctionItem AuctionItem { get; set; }
        public double BidAmount { get; set; }
        public string BidStatus { get; set; }//like winning, pending
    }
}
