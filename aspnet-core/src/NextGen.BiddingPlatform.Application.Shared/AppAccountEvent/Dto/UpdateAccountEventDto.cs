using Abp.Authorization.Users;
using NextGen.BiddingPlatform.Address.Dto;
using NextGen.BiddingPlatform.AppAccount.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NextGen.BiddingPlatform.AppAccountEvent.Dto
{
    public class UpdateAccountEventDto
    {
        public Guid UniqueId { get; set; }

        [Required]
        [MaxLength(AbpUserBase.MaxUserNameLength)]
        public string EventName { get; set; }
        [Required]
        public DateTime EventDate { get; set; }
        [Required]
        public TimeSpan EventStartTime { get; set; }
        [Required]
        public TimeSpan EventEndTime { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string EventUrl { get; set; }
        public string TimeZone { get; set; }// may be we have timezone table for this field
        public bool IsActive { get; set; }
        [Required]
        public AddressDto Address { get; set; }
    }
}
