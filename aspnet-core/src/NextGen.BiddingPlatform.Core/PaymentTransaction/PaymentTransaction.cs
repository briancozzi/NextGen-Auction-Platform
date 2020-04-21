using Abp.Domain.Entities.Auditing;
using NextGen.BiddingPlatform.Core.CardDetails;
using NextGen.BiddingPlatform.Core.Invoices;
using NextGen.BiddingPlatform.CustomInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema;

namespace NextGen.BiddingPlatform.Core.PaymentTransactions
{
    public class PaymentTransaction : FullAuditedEntity, IHasUniqueIdentifier
    {
        [Index("IX_PaymentTransaction_UniqueId", IsClustered = false, IsUnique = true)]
        public Guid UniqueId { get; set; }

        [ForeignKey("Invoice")]
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }

        [Required]
        public string PaymentStatus { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        [ForeignKey("CardDetail")]
        public int CardDetailId { get; set; }
        public CardDetail CardDetail { get; set; }
        //   public Guid OrganizationSubscriptionId { get; set; }
    }
}
