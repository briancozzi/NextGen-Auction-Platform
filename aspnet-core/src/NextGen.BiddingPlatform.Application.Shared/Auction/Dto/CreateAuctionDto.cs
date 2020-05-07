using NextGen.BiddingPlatform.Address.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NextGen.BiddingPlatform.Auction.Dto
{
    public class CreateAuctionDto
    {
        [Required]
        public Guid AccountUniqueId { get; set; }
        [Required]
        public Guid EventUniqueId { get; set; }
        [Required]
        public string AuctionType { get; set; }
        [Required]
        public DateTime AuctionStartDateTime { get; set; }
        [Required]
        public DateTime AuctionEndDateTime { get; set; }
        public AddressDto Address { get; set; }
    }
}
