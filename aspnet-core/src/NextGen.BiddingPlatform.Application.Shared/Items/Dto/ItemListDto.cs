using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.Items.Dto
{
    public class ItemListDto
    {
        public Guid UniqueId { get; set; }
        public int ItemType { get; set; }
        public int ItemNumber { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public string MainImageName { get; set; }
        public string ThumbnailImage { get; set; }
        public int ItemStatus { get; set; }
    }
}
