using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using NextGen.Auction.Configuration;
using NextGen.Auction.Web;

namespace NextGen.Auction.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class AuctionDbContextFactory : IDesignTimeDbContextFactory<AuctionDbContext>
    {
        public AuctionDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AuctionDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            AuctionDbContextConfigurer.Configure(builder, configuration.GetConnectionString(AuctionConsts.ConnectionStringName));

            return new AuctionDbContext(builder.Options);
        }
    }
}
