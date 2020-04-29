using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NextGen.BiddingPlatform.AuctionItem.Dto
{
    public class CreateAuctionItemDto
    {
        [Required]
        public Guid AuctionId { get; set; }
        [Required]
        public Guid ItemId { get; set; }
        public bool IsActive { get; set; }
    }
}
