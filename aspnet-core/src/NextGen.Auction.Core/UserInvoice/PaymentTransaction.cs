using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NextGen.Auction.UserInvoice
{
    [Table("PaymentTransactions")]
    public class PaymentTransaction : FullAuditedEntity<Guid>
    {
        [ForeignKey("Invoice")]
        public Guid InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        [ForeignKey("CardDetail")]
        public Guid CardDetailId { get; set; }
        public CardDetail CardDetail { get; set; }
        //   public Guid OrganizationSubscriptionId { get; set; }
    }
}
