using RefactorName.RepositoryInterface;
using RefactorName.SqlServerRepository.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.SqlServerRepository
{
    public class GenericWriteThroughCacheRepository : IGenericRepository
    {
        private readonly IGenericRepository repository;

        public GenericWriteThroughCacheRepository()
        {
            this.repository = new GenericRepository();
        }

        public TBusinessEntity Create<TBusinessEntity>(TBusinessEntity entity)
            where TBusinessEntity : class, new()
        {
            var result = repository.Create(entity);

            /// retrive the primary key value of business entity that just been created.
            var id = Cache.GetPrimaryKeyValue(result);

            // create standard cache key
            string cacheKey = Cache.CacheKey<TBusinessEntity>(id);

            // do the write-through caching for the created TBusinessEntity
            Cache.Provider.Set(cacheKey, result);

            return result;
        }

        public bool Delete<TBusinessEntity>(IEnumerable<TBusinessEntity> entities)
            where TBusinessEntity : class, new()
        {
            throw new NotImplementedException();
        }

        public bool Delete<TBusinessEntity>(TBusinessEntity entity)
            where TBusinessEntity : class, new()
        {
            var result = repository.Delete(entity);

            /// retrive the primary key value of business entity that just been deleted
            var id = Cache.GetPrimaryKeyValue(entity);

            // create standard cache key
            string cacheKey = Cache.CacheKey<TBusinessEntity>(id);

            // do remove write-through caching for the deleted business entity
            Cache.Provider.Clear(cacheKey);

            return result;
        }

        public TBusinessEntity Update<TBusinessEntity>(TBusinessEntity entity) where TBusinessEntity : class, new()
        {
            var result = repository.Update(entity);

            /// retrive the primary key value of business entity that just been updated
            var id = Cache.GetPrimaryKeyValue(result);

            // create standard cache key
            string cacheKey = Cache.CacheKey<TBusinessEntity>(id);

            // do the write-through caching for the update business entity
            Cache.Provider.Set(cacheKey, result);

            return result;
        }
    }
}
