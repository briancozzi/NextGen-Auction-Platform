using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NextGen.Auction.Item
{
    [Table("ItemGalleries")]
    public class ItemGallery : AuditedEntity<Guid>
    {
        [ForeignKey("Item")]
        public Guid ItemId { get; set; }
        public Item Item { get; set; }
        public string ImageName { get; set; }
    }
}
