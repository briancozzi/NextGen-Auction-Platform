using NextGen.BiddingPlatform.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.Items.Dto
{
    public class ItemFilter : PagedAndSortedInputDto
    {
        public string Search { get; set; }
    }
}
