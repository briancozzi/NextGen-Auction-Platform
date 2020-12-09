using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.AuctionHistory.Dto
{
    public class AuctionBidderHistoryDto
    {
        public string BidderName { get; set; }
        public Guid AuctionItemId { get; set; }
        public double BidAmount { get; set; }
        public int? AuctionBidderId { get; set; }
        public long UserId { get; set; }
        public int? TenantId { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid UniqueId { get; set; }
    }
    public class AuctionItemWinnerDto
    {
        public Guid AuctionItemId { get; set; }
        public double BidAmount { get; set; }
        public int? AuctionBidderId { get; set; }
        public long UserId { get; set; }
    }
}
