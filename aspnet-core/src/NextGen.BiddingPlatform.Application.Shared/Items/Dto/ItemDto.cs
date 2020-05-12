﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NextGen.BiddingPlatform.Items.Dto
{
    public class ItemDto
    {
        //[Required]
        //public int ItemType { get; set; }//may be enum or dropdown
        [Required]
        public int ItemNumber { get; set; }
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
        [Required]
        public double AcquisitionValue { get; set; }
        public double BuyNowPrice { get; set; } = 0;//we are just adding this property because in future if we have buy now button then we can use it
        public string ItemCertificateNotes { get; set; }

        [Required]
        public string MainImageName { get; set; }
        public string ThumbnailImage { get; set; }
        public string VideoLink { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<ItemGalleryDto> ItemImages { get; set; }
        public ICollection<int> Categories { get; set; }

        public ItemDto()
        {
            ItemImages = new Collection<ItemGalleryDto>();
            Categories = new Collection<int>();
        }
    }
}
