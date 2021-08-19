using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.UserfavoriteItems.Dto
{
    public class CreateOrEditFavoriteItemDto
    {
        public long UserId { get; set; }
        public Guid ItemId { get; set; }
        public bool IsFavorite { get; set; }
        public int? TenantId { get; set; }
    }
}
