using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NextGen.BiddingPlatform.AuctionBidder.Dto
{
    public class CreateAuctionBidderDto
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public Guid AuctionId { get; set; }
        [Required]
        public string BidderName { get; set; }
    }
}
