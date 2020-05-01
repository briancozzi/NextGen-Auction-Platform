using NextGen.BiddingPlatform.Address.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NextGen.BiddingPlatform.CardDetail.Dto
{
    public class UpdateCardDetailDto
    {
        [Required]
        public Guid UniqueId { get; set; }
        [Required]
        public long UserId { get; set; }

        public string FullName { get; set; }

        [Required]
        public string CreditCardNo { get; set; }
        [Required]
        public string CVV { get; set; }
        [Required]
        public string ExpiryMonth { get; set; }
        [Required]
        public string ExpiryYear { get; set; }

        public AddressDto Address { get; set; }
    }
}
