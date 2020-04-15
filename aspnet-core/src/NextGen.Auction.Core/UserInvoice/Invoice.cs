using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using NextGen.Auction.Auctions;
using NextGen.Auction.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NextGen.Auction.UserInvoice
{
    [Table("Invoices")]
    public class Invoice : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        public User User { get; set; }
        [ForeignKey("AuctionHistoryId")]
        public Guid AuctionHistoryId { get; set; }
        public AuctionHistory AuctionHistory { get; set; }
        public string InvoiceStatus { get; set; }
        public ICollection<PaymentTransaction> PaymentTransactions { get; set; }
        public Invoice()
        {
            PaymentTransactions = new Collection<PaymentTransaction>();
        }
    }
}
