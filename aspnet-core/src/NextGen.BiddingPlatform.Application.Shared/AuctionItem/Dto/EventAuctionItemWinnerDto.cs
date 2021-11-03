using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.AuctionItem.Dto
{
    public class EventAuctionItemWinnerDto
    {
        public string ItemName { get; set; }
        public decimal ItemPrice { get; set; }
        public string WinnerName { get; set; }
        public decimal LastBiddingAmountOfWinner { get; set; }
        public string AuctionStatus { get; set; }

        public Guid AuctionItemId { get; set; }
        public Guid AuctionBidderId { get; set; }
    }


    public class EventItemWinners
    {
        public EventItemWinners()
        {
            Winners = new List<EventWinnersDto>();
        }
        public string EventName { get; set; }
        public Guid EventUniqueId { get; set; }
        public List<EventWinnersDto> Winners { get; set; }
    }

    public class EventWinnersDto
    {
        public EventWinnersDto()
        {
            Items = new List<WinnerItemDto>();
        }
        public Guid BidderId { get; set; }
        public string BidderName { get; set; }
        public List<WinnerItemDto> Items { get; set; }
    }

    public class WinnerItemDto
    {
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public double ItemAmount { get; set; }
    }
}
