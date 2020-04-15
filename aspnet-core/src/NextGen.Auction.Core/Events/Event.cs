using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using NextGen.Auction.Address;

namespace NextGen.Auction.Events
{
    [Table("Events")]
    public class Event : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }//tenant as organization
        public string Name { get; set; }
        public DateTime EventDate { get; set; }
        [ForeignKey("AppAccount")]
        public Guid AppAccountId { get; set; }
        public AppAccounts.AppAccount AppAccount { get; set; }
        [ForeignKey("Address")]
        public Guid AddressId { get; set; }
        public Address.Address Address { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public ICollection<Auctions.Auction> EventAuctions { get; set; }

        public Event()
        {
            EventAuctions = new Collection<Auctions.Auction>();
            Address = new Address.Address();
        }
    }
}
