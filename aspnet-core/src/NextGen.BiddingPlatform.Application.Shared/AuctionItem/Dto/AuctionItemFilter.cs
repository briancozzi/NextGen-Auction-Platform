using NextGen.BiddingPlatform.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.AuctionItem.Dto
{
    public class AuctionItemFilter : PagedAndSortedInputDto
    {
        public string Search { get; set; }
    }
}
