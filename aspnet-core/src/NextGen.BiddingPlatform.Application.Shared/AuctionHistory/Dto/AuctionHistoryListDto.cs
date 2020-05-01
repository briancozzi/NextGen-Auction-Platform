using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.AuctionHistory.Dto
{
    public class AuctionHistoryListDto
    {
        public Guid UniqueId { get; set; }
        public Guid BidderId { get; set; }
        public Guid AuctionItemId { get; set; }
        public string BidderName { get; set; }
        public string ItemName { get; set; }
        public int ItemType { get; set; }
        public string BidStatus { get; set; }
        public double BidAmount { get; set; }
    }
}
