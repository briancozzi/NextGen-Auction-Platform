﻿using Abp.Authorization.Users;
using NextGen.BiddingPlatform.Address.Dto;
using NextGen.BiddingPlatform.AppAccount.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NextGen.BiddingPlatform.AppAccountEvent.Dto
{
    public class AccountEventDto
    {
        public Guid UniqueId { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public TimeSpan EventStartDateTime { get; set; }
        public TimeSpan EventEndDateTime { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string EventUrl { get; set; }
        public string TimeZone { get; set; }// may be we have timezone table for this field
        public bool IsActive { get; set; }
        public AddressDto Address { get; set; }
        public Guid AccountUniqueId { get; set; }
    }
}
