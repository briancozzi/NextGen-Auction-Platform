using Abp.Dependency;
using GraphQL;
using GraphQL.Types;
using NextGen.BiddingPlatform.Queries.Container;

namespace NextGen.BiddingPlatform.Schemas
{
    public class MainSchema : Schema, ITransientDependency
    {
        public MainSchema(IDependencyResolver resolver) :
            base(resolver)
        {
            Query = resolver.Resolve<QueryContainer>();
        }
    }
}