using Abp.Domain.Entities.Auditing;
using NextGen.BiddingPlatform.Core.Categories;
using NextGen.BiddingPlatform.Core.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NextGen.BiddingPlatform.ItemCategory
{
    public class ItemCategory 
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public Item Item { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
