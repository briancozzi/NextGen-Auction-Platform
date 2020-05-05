using Abp.Authorization.Users;
using NextGen.BiddingPlatform.Address.Dto;
using NextGen.BiddingPlatform.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NextGen.BiddingPlatform.AppAccount.Dto
{
    public class AppAccountDto :  PagedAndSortedInputDto
    {
        public Guid UniqueId { get; set; }
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
        public bool IsActive { get; set; }
        public AddressDto Address { get; set; }
        public string ThumbnailImage { get; set; }
    }
}
