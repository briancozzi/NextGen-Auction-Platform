using Microsoft.EntityFrameworkCore;
using NextGen.BiddingPlatform.Core.Categories;
using NextGen.BiddingPlatform.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NextGen.BiddingPlatform.Migrations.Seed.Tenants
{
    public class DefaultCategoryBuilder
    {
        private readonly BiddingPlatformDbContext _context;
        public DefaultCategoryBuilder(BiddingPlatformDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateDefaultCategories();
        }

        private void CreateDefaultCategories()
        {
            var anyCategoriesExist = _context.Categories.IgnoreQueryFilters().Any(s => s.TenantId == 1 && !s.IsDeleted);
            if (!anyCategoriesExist)
            {
                _context.Categories.AddRange(GetCategories());
                _context.SaveChanges();
            }
        }

        private List<Category> GetCategories()
        {
            List<Category> list = new List<Category>
            {
                new Category{ CategoryName = "Antiques", UniqueId = Guid.NewGuid() , TenantId = 1},
                new Category{ CategoryName = "Art", UniqueId = Guid.NewGuid(), TenantId = 1},
                new Category{ CategoryName = "Clothing", UniqueId = Guid.NewGuid(), TenantId = 1},
                new Category{ CategoryName = "Electronics", UniqueId = Guid.NewGuid(), TenantId = 1},
                new Category{ CategoryName = "Collectibles", UniqueId = Guid.NewGuid(), TenantId = 1},
                new Category{ CategoryName = "Entertainment Memorabilia", UniqueId = Guid.NewGuid(), TenantId = 1},
                new Category{ CategoryName = "Sports Memorabilia", UniqueId = Guid.NewGuid(), TenantId = 1},
                new Category{ CategoryName = "Gift Cards & Coupons", UniqueId = Guid.NewGuid(), TenantId = 1},
                new Category{ CategoryName = "Health & Beauty", UniqueId = Guid.NewGuid(), TenantId = 1},
                new Category{ CategoryName = "Home & Garden", UniqueId = Guid.NewGuid(), TenantId = 1},
                new Category{ CategoryName = "Jewelry", UniqueId = Guid.NewGuid(), TenantId = 1},
                new Category{ CategoryName = "Tickets & Experiences", UniqueId = Guid.NewGuid(), TenantId = 1},
                new Category{ CategoryName = "Travel", UniqueId = Guid.NewGuid(), TenantId = 1},
                new Category{ CategoryName = "Other", UniqueId = Guid.NewGuid(), TenantId = 1},
            };

            return list;
        }
    }
}
