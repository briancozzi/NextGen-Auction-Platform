using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.AuctionBidder.Dto
{
    public class AuctionBidderListDto
    {
        public Guid UniqueID { get; set; }
        public string BidderName { get; set; }

        public Guid AuctionUniqueId { get; set; }
        public string AuctionType { get; set; }
        public DateTime AuctionStartDateTime { get; set; }
        public DateTime AuctionEndDateTime { get; set; }
        
        public string FullName { get; set; }
        
        
    }
}
