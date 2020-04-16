using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using NextGen.BiddingPlatform.Configure;
using NextGen.BiddingPlatform.Startup;
using NextGen.BiddingPlatform.Test.Base;

namespace NextGen.BiddingPlatform.GraphQL.Tests
{
    [DependsOn(
        typeof(BiddingPlatformGraphQLModule),
        typeof(BiddingPlatformTestBaseModule))]
    public class BiddingPlatformGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BiddingPlatformGraphQLTestModule).GetAssembly());
        }
    }
}