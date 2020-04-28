using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NextGen.BiddingPlatform.Category.Dto
{
    public class CategoryListDto
    {
        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public string CategoryName { get; set; }
    }
}
