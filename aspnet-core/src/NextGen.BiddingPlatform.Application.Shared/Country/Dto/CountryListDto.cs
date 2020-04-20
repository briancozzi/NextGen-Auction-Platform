using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.Country.Dto
{
    public class CountryListDto
    {
        public Guid UniqueId { get; set; }

        public string CountryCode { get; set; }

        public string CountryName { get; set; }
    }
}
