using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace NextGen.BiddingPlatform.EntityFrameworkCore
{
    public static class BiddingPlatformDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<BiddingPlatformDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<BiddingPlatformDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}