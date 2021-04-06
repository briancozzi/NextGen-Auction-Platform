using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NextGen.BiddingPlatform.Country.Dto
{
    public class CountryDto
    {
        public const int MaxCountryCodeLength = 3;

        public int Id { get; set; }
        public Guid UniqueId { get; set; }

        [Required]
        [MaxLength(MaxCountryCodeLength)]
        public string CountryCode { get; set; }

        [Required]
        [MaxLength(AbpUserBase.MaxUserNameLength)]
        public string CountryName { get; set; }
    }
}
