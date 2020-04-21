﻿using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using NextGen.BiddingPlatform.Core.Auctions;
using NextGen.BiddingPlatform.Core.Items;
using NextGen.BiddingPlatform.CustomInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema;

namespace NextGen.BiddingPlatform.Core.AuctionItems
{
    [Table("AuctionItem")]
    public class AuctionItem : AuditedEntity,IHasUniqueIdentifier,IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Index("IX_AuctionItem_UniqueId", IsClustered = false, IsUnique = true)]
        public Guid UniqueId { get; set; }

        [ForeignKey("Auction")]
        public int AuctionId { get; set; } 
        public Auction Auction { get; set; }

        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public Item Item { get; set; }

    }
}
