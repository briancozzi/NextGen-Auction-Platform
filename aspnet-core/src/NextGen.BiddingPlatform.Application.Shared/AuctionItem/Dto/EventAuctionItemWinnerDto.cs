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

        public int AuctionItemId { get; set; }
        public int AuctionBidderId { get; set; }
    }
}
