using NextGen.BiddingPlatform.AuctionItem.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.AppAccountEvent.Dto
{
    public class EventWithAuctionItems
    {
        public EventWithAuctionItems()
        {
            AuctionItems = new List<AuctionItemListDto>();
        }
        public string EventName { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public List<AuctionItemListDto> AuctionItems { get; set; }
    }

}
