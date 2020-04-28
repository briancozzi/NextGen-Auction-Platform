using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NextGen.BiddingPlatform.Category.Dto
{
    public class CategoryListDto
    {
        public Guid UniqueId { get; set; }
        public string CategoryName { get; set; }
        public int? ParentId { get; set; }
    }
}
