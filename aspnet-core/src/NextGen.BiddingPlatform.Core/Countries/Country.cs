using Abp.Domain.Entities.Auditing;
using NextGen.BiddingPlatform.CustomInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema;

namespace NextGen.BiddingPlatform.AddressEntity
{
    [Table("Countries")]
    public class Country : AuditedEntity, IHasUniqueIdentifier
    {
        [Index("IX_Country", 1, IsUnique = true, IsClustered = false)]
        public Guid UniqueId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
    }
}
