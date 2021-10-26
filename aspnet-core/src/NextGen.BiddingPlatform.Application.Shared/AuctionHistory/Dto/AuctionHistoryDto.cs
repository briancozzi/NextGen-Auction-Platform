using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.AuctionHistory.Dto
{
    public class AuctionHistoryDto
    {
        public int AuctionBidderId { get; set; }
        public int AuctionItemId { get; set; }
        public double BidAmount { get; set; }
        public bool IsOutBid { get; set; }
        public bool IsBiddingClosed { get; set; }
    }
}
