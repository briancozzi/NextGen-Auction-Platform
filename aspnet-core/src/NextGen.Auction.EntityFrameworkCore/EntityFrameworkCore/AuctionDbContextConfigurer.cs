using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Auction.EntityFrameworkCore
{
    public static class AuctionDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<AuctionDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<AuctionDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
