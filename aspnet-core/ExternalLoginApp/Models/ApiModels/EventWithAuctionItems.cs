using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExternalLoginApp.Models.ApiModels
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
