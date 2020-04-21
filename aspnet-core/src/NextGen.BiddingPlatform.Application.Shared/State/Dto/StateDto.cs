using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NextGen.BiddingPlatform.State.Dto
{
    public class StateDto
    {
        public const int MaxStateNameLength = 25;
        public const int MaxStateCodeLength = 3;

        public Guid UniqueId { get; set; }
        [Required]
        [MaxLength(MaxStateNameLength)]
        public string StateName { get; set; }

        [Required]
        [MaxLength(MaxStateCodeLength)]
        public string StateCode { get; set; }

        [Required]
        public Guid CountryUniqueId { get; set; }
        public string CountryName { get; set; }
    }
}
