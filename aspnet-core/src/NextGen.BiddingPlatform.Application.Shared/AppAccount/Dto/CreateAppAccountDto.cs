using Abp.Authorization.Users;
using NextGen.BiddingPlatform.Address.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NextGen.BiddingPlatform.AppAccount.Dto
{
    public class CreateAppAccountDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(AbpUserBase.MaxNameLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(AbpUserBase.MaxNameLength)]
        public string LastName { get; set; }

        [Required]
        public string PhoneNo { get; set; }

        public string Logo { get; set; } // Account/Company logo

        public AddressDto Address { get; set; }

        public string ThumbnailImage { get; set; }
    }
}
