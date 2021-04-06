using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.State.Dto
{
    public class StateListDto
    {
        public Guid UniqueId { get; set; }

        public string StateName { get; set; }

        public string StateCode { get; set; }

        public Guid CountryUniqueId { get; set; }
        public string CountryName { get; set; }
    }
}
