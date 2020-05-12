using Abp.Extensions;
using Abp.Runtime.Validation;
using NextGen.BiddingPlatform.Address.Dto;
using NextGen.BiddingPlatform.Dto;
using NextGen.BiddingPlatform.Items.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.Auction.Dto
{
    public class AuctionDto
    {
        public Guid UniqueId { get; set; }
        public Guid EventUniqueId { get; set; }
        public Guid AppAccountUniqueId { get; set; }
        public string AuctionType { get; set; }
        public DateTime AuctionStartDateTime { get; set; }
        public DateTime AuctionEndDateTime { get; set; }
        public AddressDto Address { get; set; }
        public List<GetItemDto> Items { get; set; }
    }
}
