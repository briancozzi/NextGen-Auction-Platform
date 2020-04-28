using NextGen.BiddingPlatform.Category.Dto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NextGen.BiddingPlatform.Items.Dto
{
    public class GetItemDto
    {
        public Guid UniqueId { get; set; }
        public int ItemType { get; set; }
        public int ItemNumber { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public int ProcurementState { get; set; }
        public int ItemStatus { get; set; }
        public int Visibility { get; set; }
        public double FairMarketValue_FMV { get; set; }
        public double StartingBidValue { get; set; }
        public double BidStepIncrementValue { get; set; }
        public double AcquisitionValue { get; set; }
        public double BuyNowPrice { get; set; } = 0;
        public string ItemCertificateNotes { get; set; }
        public string MainImageName { get; set; }
        public string VideoLink { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<ItemGalleryDto> ItemImages { get; set; }
        public ICollection<GetCategoryDto> ItemCategories { get; set; }

        public GetItemDto()
        {
            ItemImages = new Collection<ItemGalleryDto>();
            ItemCategories = new Collection<GetCategoryDto>();
        }
    }
    public class GetCategoryDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
    }
}
