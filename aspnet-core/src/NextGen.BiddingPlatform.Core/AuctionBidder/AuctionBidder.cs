using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using NextGen.BiddingPlatform.Authorization.Users;
using NextGen.BiddingPlatform.Core.AuctionHistories;
using NextGen.BiddingPlatform.CustomInterface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema;

namespace NextGen.BiddingPlatform.Core.AuctionBidders
{
    public class AuctionBidder : FullAuditedEntity, IMustHaveTenant, IHasUniqueIdentifier
    {
        public int TenantId { get; set; }

        [Index("IX_AuctionHistory_UniqueId", IsClustered = false, IsUnique = true)]
        public Guid UniqueId { get; set; }

        [ForeignKey("User")]
        public long UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Auction")]
        public int AuctionId { get; set; }
        //public Auctions.Auction Auction { get; set; }

        [Required]
        public string BidderName { get; set; }

        public ICollection<AuctionHistory> AuctionHistories { get; set; }
        public AuctionBidder()
        {
            AuctionHistories = new Collection<AuctionHistory>();
        }
    }
}
