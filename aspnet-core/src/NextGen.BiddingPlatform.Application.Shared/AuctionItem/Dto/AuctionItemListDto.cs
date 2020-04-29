using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.AuctionItem.Dto
{
    public class AuctionItemListDto
    {
        //Auction
        public Guid AuctionId { get; set; }
        public string AuctionType { get; set; }
        public DateTime AuctionStartDateTime { get; set; }
        public DateTime AuctionEndDateTime { get; set; }

        //Item
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public int ItemType { get; set; }//may be enum or dropdown
        public int ItemNumber { get; set; }

        public bool IsActive { get; set; }
    }
}
