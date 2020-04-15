using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using NextGen.Auction.Authorization.Roles;
using NextGen.Auction.Authorization.Users;
using NextGen.Auction.MultiTenancy;
using NextGen.Auction.AppAccounts;

namespace NextGen.Auction.EntityFrameworkCore
{
    public class AuctionDbContext : AbpZeroDbContext<Tenant, Role, User, AuctionDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<AppAccount> AppAccounts { get; set; }

        public AuctionDbContext(DbContextOptions<AuctionDbContext> options)
            : base(options)
        {
        }
    }
}
