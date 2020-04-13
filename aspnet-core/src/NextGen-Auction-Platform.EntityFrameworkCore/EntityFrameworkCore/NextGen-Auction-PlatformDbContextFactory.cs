using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using NextGen-Auction-Platform.Configuration;
using NextGen-Auction-Platform.Web;

namespace NextGen-Auction-Platform.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class NextGen-Auction-PlatformDbContextFactory : IDesignTimeDbContextFactory<NextGen-Auction-PlatformDbContext>
    {
        public NextGen-Auction-PlatformDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<NextGen-Auction-PlatformDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            NextGen-Auction-PlatformDbContextConfigurer.Configure(builder, configuration.GetConnectionString(NextGen-Auction-PlatformConsts.ConnectionStringName));

            return new NextGen-Auction-PlatformDbContext(builder.Options);
        }
    }
}
