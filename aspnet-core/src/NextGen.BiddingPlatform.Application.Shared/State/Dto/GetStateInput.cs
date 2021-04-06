using NextGen.BiddingPlatform.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.State.Dto
{
    public class GetStateInput : PagedAndSortedInputDto
    {
        public string StateCode { get; set; }
    }
}
