using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using NextGen.BiddingPlatform.CustomInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema;

namespace NextGen.BiddingPlatform.Core.Categories
{
    //[Table("Categories")]
    public class Category : FullAuditedEntity, IMustHaveTenant, IHasUniqueIdentifier
    {
        public int TenantId { get; set; }

        [Index("IX_Category_UniqueId", IsClustered = false, IsUnique = true)]
        public Guid UniqueId { get; set; }

        [Required]
        public string CategoryName { get; set; }

        [ForeignKey("ParentCategoryId")]
        public int? ParentId { get; set; }
        public Category ParentCategoryId { get; set; }

    }
}
