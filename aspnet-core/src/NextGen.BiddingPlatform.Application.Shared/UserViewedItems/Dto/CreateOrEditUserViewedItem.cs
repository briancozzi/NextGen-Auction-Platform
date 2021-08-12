using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.UserViewedItems.Dto
{
    public class CreateOrEditUserViewedItem
    {
        public long UserId { get; set; }
        public Guid AuctionItemId { get; set; }
    }
}
