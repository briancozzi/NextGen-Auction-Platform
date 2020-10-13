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
    }
}
