using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using NextGen-Auction-Platform.Authorization.Roles;
using NextGen-Auction-Platform.Authorization.Users;
using NextGen-Auction-Platform.MultiTenancy;

namespace NextGen-Auction-Platform.EntityFrameworkCore
{
    public class NextGen-Auction-PlatformDbContext : AbpZeroDbContext<Tenant, Role, User, NextGen-Auction-PlatformDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public NextGen-Auction-PlatformDbContext(DbContextOptions<NextGen-Auction-PlatformDbContext> options)
            : base(options)
        {
        }
    }
}
