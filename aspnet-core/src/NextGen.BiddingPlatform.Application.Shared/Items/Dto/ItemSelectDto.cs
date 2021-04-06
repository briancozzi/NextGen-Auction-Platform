using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.Items.Dto
{
    public class ItemSelectDto
    {
        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public string ItemName { get; set; }
    }
}
