using Abp.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NextGen.BiddingPlatform.Country.Dto
{
    public class CreateCountryDto
    {
        public const int MaxCountryCodeLength = 3;

        [Required]
        [MaxLength(MaxCountryCodeLength)]
        public string CountryCode { get; set; }

        [Required]
        [MaxLength(AbpUserBase.MaxUserNameLength)]
        public string CountryName { get; set; }
    }
}
