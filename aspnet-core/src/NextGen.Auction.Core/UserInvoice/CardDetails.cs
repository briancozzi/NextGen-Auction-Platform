using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using NextGen.Auction.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NextGen.Auction.UserInvoice
{
    public class CardDetail : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        public User User { get; set; }
        public string CreditCardNo { get; set; }
        public string CVV { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        [ForeignKey("Address")]
        public Guid AddressId { get; set; }
        public Address.Address Address { get; set; }
        public ICollection<PaymentTransaction> CardTransactions { get; set; }
        public CardDetail()
        {
            CardTransactions = new Collection<PaymentTransaction>();
            Address = new Address.Address();
        }
    }
}
