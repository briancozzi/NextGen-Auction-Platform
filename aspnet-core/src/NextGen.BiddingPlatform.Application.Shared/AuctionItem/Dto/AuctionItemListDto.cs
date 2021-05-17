﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.AuctionItem.Dto
{
    public class AuctionItemListDto
    {
        public Guid AuctionItemId { get; set; }
        //Auction
        public Guid AuctionId { get; set; }
        public string AuctionType { get; set; }
        public DateTime AuctionStartDateTime { get; set; }
        public DateTime AuctionEndDateTime { get; set; }

        //Item
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public int ItemStatus { get; set; }
        public int ItemType { get; set; }//may be enum or dropdown
        public int ItemNumber { get; set; }
        public double FairMarketValue_FMV { get; set; }
        public string ImageName { get; set; }
        public string Thumbnail { get; set; }
        public string RemainingDays { get; set; }
        public string RemainingTime { get; set; }
        public double LastBidAmount { get; set; }
        public int TotalBidCount { get; set; }
        public bool IsClosedItemStatus { get; set; }

        public bool IsDaysGreaterThan1
        {
            get
            {
                if (RemainingDays == "0" || RemainingDays == "1")
                    return false;
                else
                    return true;
            }
        }
    }
}
