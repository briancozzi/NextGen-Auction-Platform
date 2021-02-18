using Abp.Dependency;
using GraphQL.Types;
using GraphQL.Utilities;
using NextGen.BiddingPlatform.Queries.Container;
using System;

namespace NextGen.BiddingPlatform.Schemas
{
    public class MainSchema : Schema, ITransientDependency
    {
        public MainSchema(IServiceProvider provider) :
            base(provider)
        {
            Query = provider.GetRequiredService<QueryContainer>();
        }
    }
}