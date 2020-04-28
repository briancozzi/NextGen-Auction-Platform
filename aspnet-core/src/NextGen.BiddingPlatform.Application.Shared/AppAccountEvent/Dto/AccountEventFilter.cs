using NextGen.BiddingPlatform.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.AppAccountEvent.Dto
{
    public class AccountEventFilter : PagedAndSortedInputDto
    {
        public string Search { get; set; }
    }
}
