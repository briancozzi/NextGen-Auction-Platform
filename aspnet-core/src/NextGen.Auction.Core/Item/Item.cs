using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NextGen.Auction.Item
{
    [Table("Items")]
    public class Item : FullAuditedEntity<Guid>
    {
        public string ItemType { get; set; }//may be an enum
        public int ItemNumber { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public string ProcurementState { get; set; }//may be an enum like In hand, shipped on address
        public string State { get; set; }// may be an enum
        public string Visibility { get; set; }//may be an enum like live
        public string TypeOfDonor { get; set; }//may be an enum like individual, company
        public long DonorUserId { get; set; }
        public string DisplayNameOnItem { get; set; }
        public string TypeOfSolicitor { get; set; }//may be an enum like individual, company
        public long SolicitorUserId { get; set; }
        public double FairMarketValue_FMV { get; set; }
        public double StartingBidValue { get; set; }
        public double BidStepIncrementValue { get; set; }
        public double AcquisitionValue { get; set; }
        public double BuyNowPrice { get; set; } = 0;
        public string ItemCertificateNotes { get; set; }
        public string MainImageName { get; set; }
        public string VideoLink { get; set; }
        public ICollection<ItemGallery> ItemImages { get; set; }
        public Item()
        {
            ItemImages = new Collection<ItemGallery>();
        }
    }
}
