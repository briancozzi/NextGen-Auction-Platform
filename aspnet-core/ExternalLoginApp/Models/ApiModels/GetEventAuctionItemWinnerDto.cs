using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExternalLoginApp.Models.ApiModels
{
    public class GetEventAuctionItemWinnerDto
    {
        public string EventName { get; set; }
        public Guid EventUniqueId { get; set; }
        public string ItemName { get; set; }
        public Guid ItemUniqueId { get; set; }
        public string ExternalUserId { get; set; }
        public long UserId { get; set; }
        public string BidderName { get; set; }
        public Guid BidderUniqueId { get; set; }
        public double BidAmount { get; set; }
    }
}
