using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NextGen.Auction.Item
{
    [Table("Items")]
    public class Item : FullAuditedEntity<Guid>
    {
        public string MainImageName { get; set; }
        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<ItemGallery> ItemImages { get; set; }
        public Item()
        {
            ItemImages = new Collection<ItemGallery>();
        }
    }
}
