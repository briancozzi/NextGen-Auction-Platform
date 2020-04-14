using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NextGen.Auction.Auctions
{
    [Table("AuctionItems")]
    public class AuctionItem : FullAuditedEntity<Guid>
    {
        [ForeignKey("Auction")]
        public Guid AuctionId { get; set; }
        public Auction Auction { get; set; }
        [ForeignKey("Item")]
        public Guid ItemId { get; set; }
        public Item.Item Item { get; set; }
        public double MinAmount { get; set; }
        public double MaxAmount { get; set; }
        public double StepIncrement { get; set; }
    }
}
