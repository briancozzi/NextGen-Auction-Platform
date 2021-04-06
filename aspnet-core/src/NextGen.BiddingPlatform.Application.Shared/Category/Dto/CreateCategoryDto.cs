using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NextGen.BiddingPlatform.Category.Dto
{
    public class CreateCategoryDto
    {
        [Required]
        public string CategoryName { get; set; }
    }

    public class CreateSubCategoryDto
    {
        public string SubCategoryName { get; set; }
        public Guid CategoryId { get; set; }
    }
}
