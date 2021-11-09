using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NextGen.BiddingPlatform.AuctionItem.Dto
{
    public class PaymentUpdateDto
    {
        public PaymentUpdateDto()
        {
            Items = new List<Guid>();
        }
        [Required]
        public Guid EventId { get; set; }
        [Required]
        public List<Guid> Items { get; set; }

        public string PaymentStatus { get; set; }
        public Guid BidderUUID { get; set; }
    }
}
