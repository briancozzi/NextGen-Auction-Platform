using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.AuctionHistory.Dto
{
    public class GetAuctionBidderHistoryDto
    {
        public int AuctionBidderId { get; set; }
        public string BidderName { get; set; }
        public int HistoryCount { get; set; }
        public Guid AuctionItemId { get; set; }
        public double LastHistoryAmount { get; set; }
    }
}
