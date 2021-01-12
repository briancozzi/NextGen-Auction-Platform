using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NextGen.BiddingPlatform.AuctionUserInvitations
{
    [Table("AuctionUserInvitations")]
    public class AuctionUserInvitation : Entity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public string Email { get; set; }
        public Guid AuctionId { get; set; }
    }
}
