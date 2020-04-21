using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using NextGen.BiddingPlatform.Authorization.Users;
using NextGen.BiddingPlatform.Core.PaymentTransactions;
using NextGen.BiddingPlatform.CustomInterface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema;

namespace NextGen.BiddingPlatform.Core.Invoices
{
    [Table("Invoices")]
    public class Invoice : FullAuditedEntity, IMustHaveTenant, IHasUniqueIdentifier
    {
        public int TenantId { get; set; }

        [Index("IX_Invoice_UniqueId", IsClustered = false, IsUnique = true)]
        public Guid UniqueId { get; set; }

        [ForeignKey("User")]
        public long UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("AuctionHistoryId")]
        public int AuctionHistoryId { get; set; }
        //public AuctionHistory AuctionHistory { get; set; }

        [Required]
        public string InvoiceStatus { get; set; }

        public ICollection<PaymentTransaction> PaymentTransactions { get; set; }
        public Invoice()
        {
            PaymentTransactions = new Collection<PaymentTransaction>();
        }
    }
}
