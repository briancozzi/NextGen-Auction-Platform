using NextGen.BiddingPlatform.Address.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.Auction.Dto
{
    public class AuctionListDto
    {
        public Guid UniqueId { get; set; }//auction unique id
        public Guid EventUniqueId { get; set; }
        public Guid AppAccountUniqueId { get; set; }
        public string AuctionType { get; set; }
        public DateTime AuctionStartDateTime { get; set; }
        public DateTime AuctionEndDateTime { get; set; }
    }
}
