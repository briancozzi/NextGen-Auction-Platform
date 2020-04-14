using Abp.Domain.Entities.Auditing;
using NextGen.Auction.Address;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NextGen.Auction.Address
{
    [Table("Addresses")]
    public class Address : FullAuditedEntity<Guid>
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        [ForeignKey("State")]
        public Guid StateId { get; set; }
        public State State { get; set; }
        public string ZipCode { get; set; }
    }
}
