using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NextGen.BiddingPlatform.AuctionItem.Dto
{
    public class PaymentUpdateDto
    {
        [Required]
        public Guid EventId { get; set; }
        [Required]
        public Guid ItemId { get; set; }

        public string PaymentStatus { get; set; }
    }
}
