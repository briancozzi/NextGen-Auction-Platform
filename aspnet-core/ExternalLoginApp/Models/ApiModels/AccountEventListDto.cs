using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExternalLoginApp.Models.ApiModels
{

    public class AuctionEvents
    {
        public AuctionEvents()
        {
            Items = new List<AccountEventListDto>();
        }
        public List<AccountEventListDto> Items { get; set; }
    }
    public class AccountEventListDto
    {
        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public Guid AppAccountUniqueId { get; set; }
        public string EventName { get; set; }
        public DateTime EventStartDateTime { get; set; }
        public DateTime EventEndDateTime { get; set; }
        public string EventUrl { get; set; }
        public string TimeZone { get; set; }// may be we have timezone table for this field
    }
}
