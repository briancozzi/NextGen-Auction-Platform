using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NextGen.Auction.AppAccounts
{
    [Table("AppAccounts")]
    public class AppAccount : Entity<Guid>
    {
        [Required]
        public string Name { get; protected set; }
        [Required]
        public string MobileNo { get; protected set; }
        [Required]
        public string Email { get; protected set; }
    }
}
