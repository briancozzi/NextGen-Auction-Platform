using Abp.Domain.Entities;
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
    public class AuctionBidder : AuditedEntity<Guid>,IMustHaveTenant
    {
        public int TenantId { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        public User User { get; set; }
        [ForeignKey("Auction")]
        public Guid AuctionId { get; set; }
        public Auctions.Auction Auction { get; set; }
        public string BidderName { get; set; }
        public ICollection<AuctionHistory> AuctionHistories { get; set; }
        public AuctionBidder()
        {
            AuctionHistories = new Collection<AuctionHistory>();
        }
    }
}
