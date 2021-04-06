using NextGen.BiddingPlatform.Address.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.CardDetail.Dto
{
    public class CardDetailListDto
    {
        public Guid UniqueId { get; set; }
        public Guid UserUniqueId { get; set; }
        public string FullName { get; set; }
        public string CreditCardNo { get; set; }
        public string CVV { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public AddressDto Address { get; set; }
    }
}
