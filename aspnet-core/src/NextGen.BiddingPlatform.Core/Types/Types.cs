using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using NextGen.BiddingPlatform.CustomInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema;

namespace NextGen.BiddingPlatform.Core.Types
{
    public class Types : AuditedEntity, IMustHaveTenant, IHasUniqueIdentifier
    {
        public int TenantId { get; set; }

        [Index("IX_ItemType_UniqueId", IsClustered = false, IsUnique = true)]
        public Guid UniqueId { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
