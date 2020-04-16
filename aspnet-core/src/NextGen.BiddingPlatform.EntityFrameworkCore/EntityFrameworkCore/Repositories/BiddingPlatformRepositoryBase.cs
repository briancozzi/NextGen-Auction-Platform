using Abp.Domain.Entities;
using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Repositories;

namespace NextGen.BiddingPlatform.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// Base class for custom repositories of the application.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key type of the entity</typeparam>
    public abstract class BiddingPlatformRepositoryBase<TEntity, TPrimaryKey> : EfCoreRepositoryBase<BiddingPlatformDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected BiddingPlatformRepositoryBase(IDbContextProvider<BiddingPlatformDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add your common methods for all repositories
    }

    /// <summary>
    /// Base class for custom repositories of the application.
    /// This is a shortcut of <see cref="BiddingPlatformRepositoryBase{TEntity,TPrimaryKey}"/> for <see cref="int"/> primary key.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public abstract class BiddingPlatformRepositoryBase<TEntity> : BiddingPlatformRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected BiddingPlatformRepositoryBase(IDbContextProvider<BiddingPlatformDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)!!!
    }
}
