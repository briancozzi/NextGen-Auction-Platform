using NextGen.BiddingPlatform.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NextGen.BiddingPlatform.Category.Dto
{
    public class CategoryFilter : PagedAndSortedInputDto
    {
        public string Search { get; set; }
    }
}
