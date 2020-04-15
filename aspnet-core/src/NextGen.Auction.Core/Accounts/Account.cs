using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NextGen.Auction.Accounts
{
    [Table("Accounts")]
    public class Account : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public string Name { get; protected set; }
        public int TenantId { get; set; }//tenant as organization
        public string CountryCodeForMobile { get; protected set; }
        public string MobileNo { get; protected set; }
        public string Email { get; protected set; }
        public bool IsEmailVerified { get; protected set; }
        public bool IsMobileVerified { get; protected set; }
    }
}
