using NextGen.BiddingPlatform.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.Auction.Dto
{
    public class AuctionTypeFilter : PagedAndSortedInputDto
    {
        public string AuctionType { get; set; }
    }
}
