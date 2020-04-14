using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NextGen.Auction.Item
{
    [Table("Categories")]
    public class Category : AuditedEntity<Guid>
    {
        public string CategoryName { get; set; }
        public string CategoryType { get; set; }
    }
}
