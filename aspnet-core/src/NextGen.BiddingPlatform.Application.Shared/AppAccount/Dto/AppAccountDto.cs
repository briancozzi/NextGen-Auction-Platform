using Abp.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NextGen.BiddingPlatform.AppAccount.Dto
{
    public class AppAccountDto
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

        public int MyProperty { get; set; }

        [Required]
        public Guid AddressUniqueId { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public Guid StateUniqueId { get; set; }

        public Guid CountryUniqueId { get; set; }

        public string ZipCode { get; set; }


    }
}
