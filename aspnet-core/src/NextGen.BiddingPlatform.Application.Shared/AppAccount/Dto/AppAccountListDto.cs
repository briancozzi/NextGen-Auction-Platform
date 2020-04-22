using Abp.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NextGen.BiddingPlatform.AppAccount.Dto
{
    public class AppAccountListDto
    {
        public Guid UniqueId { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNo { get; set; }

        public string Logo { get; set; } // Account/Company logo

        public bool IsActive { get; set; }

        public Guid AddressUniqueId { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public int StateUniqueId { get; set; }

        public int CountryUniqueId { get; set; }

        public string ZipCode { get; set; }
    }
}
