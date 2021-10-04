using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NextGen.BiddingPlatform.AuctionBidder.Dto
{
    public class CreateAuctionBidderWithExternalApp
    {
        [Required]
        public string ExternalUserId { get; set; }
        [Required]
        public int EventId { get; set; }
    }
}
