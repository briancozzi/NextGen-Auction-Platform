using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using NextGen.BiddingPlatform.Authorization.Users;
using NextGen.BiddingPlatform.CustomInterface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Toolbelt.ComponentModel.DataAnnotations.Schema;

namespace NextGen.BiddingPlatform.Core.Items
{
    [Table("Items")]
    public class Item : FullAuditedEntity, IMustHaveTenant, IHasUniqueIdentifier
    {
        public int TenantId { get; set; }
        [Index("IX_Item_UniqueId", IsClustered = false, IsUnique = true)]
        public Guid UniqueId { get; set; }

        public int ItemType { get; set; }//leave it for now
        [Required]
        public int ItemNumber { get; set; }//random number generate
        [Required]
        public string ItemName { get; set; }
        [Required]
        public string Description { get; set; }
        public int ProcurementState { get; set; }//may be an enum like In hand, shipped on address
        public int ItemStatus { get; set; }// may be an enum like open,closed
        public int Visibility { get; set; }//may be an enum like live

        [Required]
        public double FairMarketValue_FMV { get; set; }
        [Required]
        public double StartingBidValue { get; set; }
        [Required]
        public double BidStepIncrementValue { get; set; }
        public double AcquisitionValue { get; set; }
        public double BuyNowPrice { get; set; } = 0;//we are just adding this property because in future if we have buy now button then we can use it
        public string ItemCertificateNotes { get; set; }
        [Required]
        public string MainImageName { get; set; }
        public string ThumbnailImage { get; set; }
        public string VideoLink { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("AppAccount")]
        public int AppAccountId { get; set; }
        public Core.AppAccounts.AppAccount AppAccount { get; set; }

        public ICollection<ItemGallery> ItemImages { get; set; }
        public ICollection<ItemCategory.ItemCategory> ItemCategories { get; set; }

        public Item()
        {
            ItemImages = new Collection<ItemGallery>();
            ItemCategories = new Collection<ItemCategory.ItemCategory>();
        }
    }
}
