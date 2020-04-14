using Abp.Domain.Entities.Auditing;
using NextGen.Auction.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NextGen.Auction.Auctions
{
    [Table("AuctionBidders")]
    public class AuctionBidder : AuditedEntity<Guid>
    {
        [ForeignKey("User")]
        public long UserId { get; set; }
        public User User { get; set; }
        public string BidderName { get; set; }
        public ICollection<AuctionHistory> AuctionHistories { get; set; }
        public AuctionBidder()
        {
            AuctionHistories = new Collection<AuctionHistory>();
        }
    }
}
