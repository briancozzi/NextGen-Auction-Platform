using Abp.Domain.Entities.Auditing;
using NextGen.Auction.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NextGen.Auction.Auctions
{
    [Table("Auctions")]
    public class Auction : FullAuditedEntity<Guid>
    {
        [ForeignKey("Event")]
        public Guid EventId { get; set; }
        public Event Event { get; set; }
        public Guid CompanyId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        [ForeignKey("Address")]
        public Guid AddressId { get; set; }
        public Address.Address Address { get; set; }
        public Auction()
        {
            Address = new Address.Address();
        }
    }
}
