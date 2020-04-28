using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NextGen.BiddingPlatform.Category.Dto
{
    public class CreateSubCategory
    {
        public Guid UniqueId { get; set; }

        [Required]
        public string CategoryName { get; set; }
    }
}
