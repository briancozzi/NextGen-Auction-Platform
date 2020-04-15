using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NextGen.Auction.AppAccounts
{
    [Table("AppAccounts")]
    public class AppAccount : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }//tenant as organization
        [Required]
        public string Name { get; set; }
        [Required]
        public string MobileNo { get; set; }
        [Required]
        public string Email { get; set; }
        [ForeignKey("Address")]
        public Guid AddressId { get; set; }
        public Address.Address Address { get; set; }
        public ICollection<Events.Event> AppEvents { get; set; }
        public AppAccount()
        {
            AppEvents = new Collection<Events.Event>();
        }
    }
}
