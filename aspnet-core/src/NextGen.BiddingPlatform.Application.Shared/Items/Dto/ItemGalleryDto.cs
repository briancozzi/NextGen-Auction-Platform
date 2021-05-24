using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NextGen.BiddingPlatform.Items.Dto
{
    public class ItemGalleryDto
    {
        public int Id { get; set; }
        [Required]
        public string ImageName { get; set; }

        public string Thumbnail { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
