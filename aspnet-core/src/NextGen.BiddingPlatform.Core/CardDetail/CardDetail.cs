using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using NextGen.BiddingPlatform.Authorization.Users;
using NextGen.BiddingPlatform.Core.Addresses;
using NextGen.BiddingPlatform.Core.PaymentTransactions;
using NextGen.BiddingPlatform.CustomInterface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema;

namespace NextGen.BiddingPlatform.Core.CardDetails
{
    [Table("CardDetails")]
    public class CardDetail : FullAuditedEntity, IMustHaveTenant, IHasUniqueIdentifier
    {
        public int TenantId { get; set; }

        [Index("IX_CardDetail_UniqueId", IsClustered = false, IsUnique = true)]
        public Guid UniqueId { get; set; }

        [ForeignKey("User")]
        public long UserId { get; set; }
        public User User { get; set; }

        [Required]
        public string CreditCardNo { get; set; }
        [Required]
        public string CVV { get; set; }
        [Required]
        public string ExpiryMonth { get; set; }
        [Required]
        public string ExpiryYear { get; set; }

        [ForeignKey("Address")]
        public int AddressId { get; set; } 
        public Address Address { get; set; }

        public ICollection<PaymentTransaction> CardTransactions { get; set; }

        public CardDetail()
        {
            CardTransactions = new Collection<PaymentTransaction>();
            Address = new Address();
        }
    }
}
