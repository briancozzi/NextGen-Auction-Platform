using Microsoft.Extensions.DependencyInjection;
using NextGen.BiddingPlatform.HealthChecks;

namespace NextGen.BiddingPlatform.Web.HealthCheck
{
    public static class AbpZeroHealthCheck
    {
        public static IHealthChecksBuilder AddAbpZeroHealthCheck(this IServiceCollection services)
        {
            var builder = services.AddHealthChecks();
            builder.AddCheck<BiddingPlatformDbContextHealthCheck>("Database Connection");
            builder.AddCheck<BiddingPlatformDbContextUsersHealthCheck>("Database Connection with user check");
            builder.AddCheck<CacheHealthCheck>("Cache");

            // add your custom health checks here
            // builder.AddCheck<MyCustomHealthCheck>("my health check");

            return builder;
        }
    }
}
