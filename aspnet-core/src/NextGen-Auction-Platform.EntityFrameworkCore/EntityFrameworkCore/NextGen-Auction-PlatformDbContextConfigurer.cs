using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace NextGen-Auction-Platform.EntityFrameworkCore
{
    public static class NextGen-Auction-PlatformDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<NextGen-Auction-PlatformDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<NextGen-Auction-PlatformDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
