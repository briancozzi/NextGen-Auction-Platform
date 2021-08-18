using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.UserfavoriteItems.Dto
{
    public class GetUserFavoriteItemDto
    {
        public long UserId { get; set; }
        public int ItemId { get; set; }
    }
}
