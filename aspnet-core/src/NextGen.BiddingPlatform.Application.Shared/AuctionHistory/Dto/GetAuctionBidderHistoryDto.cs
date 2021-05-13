using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.AuctionHistory.Dto
{
    public class GetAuctionBidderHistoryDto
    {
        public GetAuctionBidderHistoryDto()
        {
            AuctionItemHistory = new List<GetAuctionHistoryByAuctionIdDto>();
        }
        public int AuctionBidderId { get; set; }
        public string BidderName { get; set; }
        public int HistoryCount { get; set; }
        public Guid AuctionItemId { get; set; }
        public double LastHistoryAmount { get; set; }
        public int? TenantId { get; set; }
        public long UserId { get; set; }
        public double NextBidValue { get; set; }
        public List<GetAuctionHistoryByAuctionIdDto> AuctionItemHistory { get; set; }
    }
}
