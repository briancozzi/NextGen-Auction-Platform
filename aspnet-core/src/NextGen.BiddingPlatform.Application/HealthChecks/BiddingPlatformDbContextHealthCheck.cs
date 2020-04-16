using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NextGen.BiddingPlatform.EntityFrameworkCore;

namespace NextGen.BiddingPlatform.HealthChecks
{
    public class BiddingPlatformDbContextHealthCheck : IHealthCheck
    {
        private readonly DatabaseCheckHelper _checkHelper;

        public BiddingPlatformDbContextHealthCheck(DatabaseCheckHelper checkHelper)
        {
            _checkHelper = checkHelper;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_checkHelper.Exist("db"))
            {
                return Task.FromResult(HealthCheckResult.Healthy("BiddingPlatformDbContext connected to database."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("BiddingPlatformDbContext could not connect to database"));
        }
    }
}
