using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using NextGen.BiddingPlatform.Core.Addresses;
using NextGen.BiddingPlatform.Core.AppAccountEvents;
using NextGen.BiddingPlatform.CustomInterface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema;

namespace NextGen.BiddingPlatform.Core.AppAccounts
{
    [Table("AppAccounts")]
    public class AppAccount : FullAuditedEntity, IMustHaveTenant, IHasUniqueIdentifier
    {
        public int TenantId { get; set; }

        [Index("IX_AppAccount_UniqueId", IsClustered = false, IsUnique = true)]
        public Guid UniqueId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(AbpUserBase.MaxNameLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(AbpUserBase.MaxNameLength)]
        public string LastName { get; set; }

        [Required]
        public string PhoneNo { get; set; }

        public string Logo { get; set; } // Account/Company logo

        public bool IsActive { get; set; }

        [ForeignKey("Address")]
        public int AddressId { get; set; }
        public Address Address { get; set; }

        public ICollection<Event> AppAccountEvents { get; set; }

        public AppAccount()
        {
            AppAccountEvents = new Collection<Event>();
            Address = new Address();
        }

    }
}
