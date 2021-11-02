using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExternalLoginApp.Models.ApiModels
{
    public class EventItemWinners
    {
        public EventItemWinners()
        {
            Winners = new List<EventWinnersDto>();
        }
        public string EventName { get; set; }
        public Guid EventUniqueId { get; set; }
        public List<EventWinnersDto> Winners { get; set; }
    }

    public class EventWinnersDto
    {
        public EventWinnersDto()
        {
            Items = new List<WinnerItemDto>();
        }
        public Guid BidderId { get; set; }
        public string BidderName { get; set; }
        public List<WinnerItemDto> Items { get; set; }
    }

    public class WinnerItemDto
    {
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
    }
}
