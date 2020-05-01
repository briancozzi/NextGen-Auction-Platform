using NextGen.BiddingPlatform.Address.Dto;
using NextGen.BiddingPlatform.AppAccount.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.AppAccountEvent.Dto
{
    public class AccountEventListDto
    {
        public Guid UniqueId { get; set; }
        public Guid AccountUniqueId { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime EventStartTime { get; set; }
        public DateTime EventEndTime { get; set; }
        public string EventUrl { get; set; }
        public string TimeZone { get; set; }// may be we have timezone table for this field
    }
}
