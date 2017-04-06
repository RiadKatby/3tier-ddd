using RefactorName.Core;
using RefactorName.GraphDiff;
using RefactorName.RepositoryInterface;
using RefactorName.RepositoryInterface.Queries;
using RefactorName.SqlServerRepository.Caching;
using RefactorName.SqlServerRepository.Queries;
using System;
using System.Diagnostics;
using System.Linq;

namespace RefactorName.SqlServerRepository
{
   public class GenericQueryRepository : IGenericQueryRepository
    {
        #region IGenericQueryRepository Members

        public TEntity Single<TEntity>(int id)
            where TEntity : class, new()
        {
            using (AppDbContext context = new AppDbContext())
            {
                try
                {
                    return context.Set<TEntity>().Find(id);
                }
                catch (Exception ex)
                {
                    throw ThrowHelper.ReThrow<TEntity>(ex);
                }
            }
        }

        public TEntity Single<TEntity>(IQueryConstraints<TEntity> constraints)
            where TEntity : class, new()
        {
            using (AppDbContext context = new AppDbContext())
            {
                try
                {
                    return context.LoadAggregate(constraints.Predicate);
                }
                catch (Exception ex)
                {
                    throw ThrowHelper.ReThrow<TEntity>(ex);
                }
            }
        }

        public TEntity SingleOrDefault<TEntity>(IQueryConstraints<TEntity> constraints)
            where TEntity : class, new()
        {
            using (AppDbContext context = new AppDbContext())
            {
                try
                {
                    return context.Set<TEntity>()
                                  .ToSearchResult<TEntity>(constraints)
                                  .Items
                                  .FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ThrowHelper.ReThrow<TEntity>(ex);
                }
            }
        }

        public int GetCount<TEntity>()
            where TEntity : class, new()
        {
            using (AppDbContext context = new AppDbContext())
            {
                try
                {
                    return context.Set<TEntity>()
                                  .Count();
                }
                catch (Exception ex)
                {
                    throw ThrowHelper.ReThrow<TEntity>(ex);
                }
            }
        }

        public int GetCount<TEntity>(IQueryConstraints<TEntity> constraints)
            where TEntity : class, new()
        {
            using (AppDbContext context = new AppDbContext())
            {
                try
                {
                    return context.Set<TEntity>()
                                  .Count(constraints.Predicate);
                }
                catch (Exception ex)
                {
                    throw ThrowHelper.ReThrow<TEntity>(ex);
                }
            }
        }

        public IQueryResult<TEntity> Find<TEntity>(IQueryConstraints<TEntity> constraints)
            where TEntity : class, new()
        {
            using (AppDbContext context = new AppDbContext())
            {
                try
                {
                    IQueryResult<TEntity> results = null;

                    // get the appropriate cache key for this query
                    string key = Cache.CacheKey(constraints);

                    // check if it's in the cache already
                    if (!Cache.Provider.TryGet(key, out results))
                    {
                        Debug.WriteLine($"NOT FOUND cache for: {key}");

                        // if not, then run the query
                        results = context.Set<TEntity>().ToSearchResult(constraints);

                        // and cache the results for next time
                        Cache.Provider.Set(key, results);
                    }
                    else
                    {
                        Debug.WriteLine($"Found cache for: {key}");
                    }

                    // return the results either from cache or that were just run
                    return results;
                }
                catch (Exception ex)
                {
                    throw ThrowHelper.ReThrow<TEntity>(ex);
                }
            }
        }

        #endregion
    }
}
