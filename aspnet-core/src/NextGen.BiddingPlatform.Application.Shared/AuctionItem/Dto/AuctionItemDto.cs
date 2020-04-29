using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NextGen.BiddingPlatform.AuctionItem.Dto
{
    public class AuctionItemDto
    {
        public Guid UniqueId { get; set; }
        public Guid AuctionId { get; set; }
        public Guid ItemId { get; set; }
    }
}
