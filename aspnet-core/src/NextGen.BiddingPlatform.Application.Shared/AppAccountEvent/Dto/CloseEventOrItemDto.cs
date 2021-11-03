using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.AppAccountEvent.Dto
{
    public class CloseEventOrItemDto
    {
        public CloseEventOrItemDto()
        {
            AuctionItemIds = new List<Guid>();
        }
        public List<Guid> AuctionItemIds { get; set; }
        public int? TenantId { get; set; }
        public string FromService { get; set; }
    }
}
