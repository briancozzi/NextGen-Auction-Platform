using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NextGen.BiddingPlatform.AuctionHistory.Dto
{
    public class CreateAuctionHistoryDto
    {
        public Guid AuctionBidderId { get; set; }
        public Guid AuctionItemId { get; set; }
        [Required]
        public double BidAmount { get; set; }
        [Required]
        public string BidStatus { get; set; }
    }
}
