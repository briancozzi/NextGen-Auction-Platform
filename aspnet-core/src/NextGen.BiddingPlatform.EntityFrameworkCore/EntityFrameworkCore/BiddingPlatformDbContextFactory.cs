using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using NextGen.BiddingPlatform.Configuration;
using NextGen.BiddingPlatform.Web;

namespace NextGen.BiddingPlatform.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class BiddingPlatformDbContextFactory : IDesignTimeDbContextFactory<BiddingPlatformDbContext>
    {
        public BiddingPlatformDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<BiddingPlatformDbContext>();
            var configuration = AppConfigurations.Get(
                WebContentDirectoryFinder.CalculateContentRootFolder(),
                addUserSecrets: true
            );

            BiddingPlatformDbContextConfigurer.Configure(builder, configuration.GetConnectionString(BiddingPlatformConsts.ConnectionStringName));

            return new BiddingPlatformDbContext(builder.Options);
        }
    }
}