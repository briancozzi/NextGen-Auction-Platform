using NextGen.BiddingPlatform.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.Country.Dto
{
    public class GetCountryInput : PagedAndSortedInputDto
    {
        public string CountryName { get; set; }
    }
}
