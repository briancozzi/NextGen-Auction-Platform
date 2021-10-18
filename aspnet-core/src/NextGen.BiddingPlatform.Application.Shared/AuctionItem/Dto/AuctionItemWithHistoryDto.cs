using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.AuctionItem.Dto
{
    public class AuctionItemWithHistoryDto
    {
        public AuctionItemWithHistoryDto()
        {
            AuctionItemHistories = new List<AuctionItemHistoryDto>();
        }
        public Guid AuctionItemId { get; set; }
        //Auction
        public Guid AuctionId { get; set; }
        public string AuctionType { get; set; }
        public DateTime AuctionStartDateTime { get; set; }
        public DateTime AuctionEndDateTime { get; set; }

        //Item
        public Guid ItemId { get; set; }
        public int ItemStatus { get; set; }
        public bool IsClosedItemStatus { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public int ItemType { get; set; }//may be enum or dropdown
        public int ItemNumber { get; set; }
        public double FairMarketValue_FMV { get; set; }
        public string ImageName { get; set; }
        public string Thumbnail { get; set; }
        public string RemainingDays { get; set; }
        public string RemainingTime { get; set; }
        public double LastBidAmount { get; set; }
        public string LastBidWinnerName { get; set; }
        public int TotalBidCount { get; set; }
        public double BidStepIncrementValue { get; set; }
        public int CurrentUserAuctionHistoryCount { get; set; }
        public string CurrUserBidderName { get; set; }
        public int CurrUserBiddingId { get; set; }
        public bool IsAuctionExpired { get; set; }
        public bool IsLastBidByCurrentUser { get; set; }

        public List<AuctionItemHistoryDto> AuctionItemHistories { get; set; }

        public bool IsBiddingStarted { get; set; }
        public bool IsBiddingClosed { get; set; }
    }

    public class AuctionItemHistoryDto
    {
        public string BidderName { get; set; } = "Annonymous";
        public double BidAmount { get; set; }
        public DateTime BidDate { get; set; }
        public string BiddingDate { get; set; }
        public string BiddingTime { get; set; }
        public long AuctionBidderUserId { get; set; }
        public int AuctionBidderId { get; set; }
    }
}
