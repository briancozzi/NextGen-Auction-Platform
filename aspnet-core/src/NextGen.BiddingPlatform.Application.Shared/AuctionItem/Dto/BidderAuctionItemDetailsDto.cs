using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.AuctionItem.Dto
{
    public class BidderAuctionItemDetailsDto
    {
        public string EventName { get; set; }
        public Guid EventId { get; set; }

        public DateTime AuctionStartDate { get; set; }
        public DateTime AuctionEndDate { get; set; }


        public string ItemName { get; set; }
        public Guid ItemId { get; set; }
        public double Amount { get; set; }
        public string PaymentStatus { get; set; }
    }
}
