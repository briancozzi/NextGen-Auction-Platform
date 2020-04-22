using Abp.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NextGen.BiddingPlatform.Address.Dto
{
    public class AddressDto
    {
        public const int MaxCityLength = 25;
        public const int MaxZipCodeLength = 5;


        [Required]
        [MaxLength(AbpUserBase.MaxUserNameLength)]
        public string Address1 { get; set; }

        [MaxLength(AbpUserBase.MaxUserNameLength)]
        public string Address2 { get; set; }

        [Required]
        [MaxLength(MaxCityLength)]
        public string City { get; set; }

        [Required]
        public Guid StateUniqueId { get; set; }

        [Required]
        public Guid CountryUniqueId { get; set; }

        [Required]
        [MaxLength(MaxZipCodeLength)]
        public string ZipCode { get; set; }

    }
}
