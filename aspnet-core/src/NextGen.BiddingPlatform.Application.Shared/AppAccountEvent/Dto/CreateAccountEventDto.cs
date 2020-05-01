using Abp.Authorization.Users;
using NextGen.BiddingPlatform.Address.Dto;
using NextGen.BiddingPlatform.AppAccount.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NextGen.BiddingPlatform.AppAccountEvent.Dto
{
    public class CreateAccountEventDto
    {
        [Required]
        public Guid AppAccountUniqueId { get; set; }
        [Required]
        [MaxLength(AbpUserBase.MaxUserNameLength)]
        public string EventName { get; set; }
        //[Required]
        //public DateTime EventDate { get; set; }
        [Required]
        public DateTime EventStartDateTime { get; set; }
        [Required]
        public DateTime EventEndDateTime { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string EventUrl { get; set; }
        [Required]
        public string TimeZone { get; set; }// may be we have timezone table for this field
        public bool IsActive { get; set; } = true;
        [Required]
        public AddressDto Address { get; set; }
    }
}
