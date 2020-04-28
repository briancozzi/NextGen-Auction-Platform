using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.Country.Dto
{
    public class CountryStateDto
    {
        public CountryStateDto()
        {
            States = new List<CountryStateListDto>();
        }
        public Guid CountryUniqueId { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }

        public List<CountryStateListDto> States { get; set; }
    }

    public class CountryStateListDto
    {
        public Guid StateUniqueId { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
    }
}
