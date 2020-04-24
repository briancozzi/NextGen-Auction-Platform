using NextGen.BiddingPlatform.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.AppAccount.Dto
{
    public class AppAccountFilter : PagedAndSortedInputDto
    {
        public string SearchName { get; set; }
    }
}
