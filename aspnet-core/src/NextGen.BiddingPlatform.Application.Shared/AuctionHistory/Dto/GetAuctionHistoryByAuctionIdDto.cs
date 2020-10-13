using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.AuctionHistory.Dto
{
    public class GetAuctionHistoryByAuctionIdDto
    {
        public double BidAmount { get; set; }
        public string BidderName { get; set; }
        public string BiddingDate { get; set; }
        public string BiddingTime { get; set; }
        public DateTime BidHistoryDate { get; set; }
    }
}
