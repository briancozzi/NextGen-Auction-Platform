using NextGen.BiddingPlatform.Address.Dto;
using NextGen.BiddingPlatform.AppAccount.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.AppAccountEvent.Dto
{
    public class AccountEventListDto
    {
        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public Guid AppAccountUniqueId { get; set; }
        public string EventName { get; set; }
        //public DateTime EventDate { get; set; }
        public DateTime EventStartDateTime { get; set; }
        public DateTime EventEndDateTime { get; set; }
        public string EventUrl { get; set; }
        public string TimeZone { get; set; }// may be we have timezone table for this field
        public int ItemCount { get; set; }
    }
}
