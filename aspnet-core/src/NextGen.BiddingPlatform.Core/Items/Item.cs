using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using NextGen.BiddingPlatform.Core.Categories;
using NextGen.BiddingPlatform.CustomInterface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Toolbelt.ComponentModel.DataAnnotations.Schema;

namespace NextGen.BiddingPlatform.Core.Items
{
    public class Item : FullAuditedEntity, IMustHaveTenant, IHasUniqueIdentifier
    {
        public int TenantId { get; set; }

        [Index("IX_Item_UniqueId", IsClustered = false, IsUnique = true)]
        public Guid UniqueId { get; set; }

        [Required]
        public string ItemType { get; set; }//may be an enum
        [Required]
        public int ItemNumber { get; set; }
        [Required]
        public string ItemName { get; set; }
        [Required]
        public string Description { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public string ProcurementState { get; set; }//may be an enum like In hand, shipped on address
        public string State { get; set; }// may be an enum
        public string Visibility { get; set; }//may be an enum like live

        [Required]
        public string TypeOfDonor { get; set; }//may be an enum like individual, company
        [Required]
        public long DonorUserId { get; set; }
        public string DisplayNameOnItem { get; set; }
        public string TypeOfSolicitor { get; set; }//may be an enum like individual, company
        [Required]
        public long SolicitorUserId { get; set; }
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
        public string VideoLink { get; set; }

        public ICollection<ItemGallery> ItemImages { get; set; }
        public Item()
        {
            ItemImages = new Collection<ItemGallery>();
        }
    }
}
