using Abp.Authorization.Users;
using NextGen.BiddingPlatform.Address.Dto;
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
        public string ThumbnailImage { get; set; }
    }
}
