using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.Auction.Dto
{
   public class AuctionSelectDto
    {
        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public string AuctionType { get; set; }
    }
}
