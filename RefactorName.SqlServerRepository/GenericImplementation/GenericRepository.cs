using RefactorName.GraphDiff;
using RefactorName.RepositoryInterface;
using RefactorName.SqlServerRepository.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.SqlServerRepository
{
    public class GenericRepository : IGenericRepository
    {
        #region IGenericRepository Members

        public virtual TBusinessEntity Create<TBusinessEntity>(TBusinessEntity entity)
            where TBusinessEntity : class, new()
        {
            try
            {
                using (AppDbContext context = new AppDbContext())
                {
                    TBusinessEntity updatedEntity = context.UpdateGraph(entity);

                    int erc = context.SaveChanges();

                    var result = erc > 0 ? updatedEntity : null;

                    if (result != null)
                    {
                        // update the generation
                        Cache.NextGeneration<TBusinessEntity>();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ThrowHelper.ReThrow<TBusinessEntity>(ex);
            }
        }

        public virtual TBusinessEntity Update<TBusinessEntity>(TBusinessEntity entity)
            where TBusinessEntity : class, new()
        {
            try
            {
                using (AppDbContext context = new AppDbContext())
                {
                    TBusinessEntity updatedEntity = context.UpdateGraph<TBusinessEntity>(entity);

                    int erc = context.SaveChanges();

                    var result = erc > 0 ? updatedEntity : null;

                    if (result != null)
                    {
                        // update the generation
                        Cache.NextGeneration<TBusinessEntity>();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ThrowHelper.ReThrow<TBusinessEntity>(ex);
            }
        }

        public virtual bool Delete<TBusinessEntity>(TBusinessEntity entity)
            where TBusinessEntity : class, new()
        {
            try
            {
                using (AppDbContext context = new AppDbContext())
                {
                    context.Set<TBusinessEntity>().Attach(entity);
                    context.Set<TBusinessEntity>().Remove(entity);

                    int erc = context.SaveChanges();

                    var result = erc > 0;

                    if (result == true)
                    {
                        // update the generation
                        Cache.NextGeneration<TBusinessEntity>();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ThrowHelper.ReThrow<TBusinessEntity>(ex);
            }
        }

        public bool Delete<TBusinessEntity>(IEnumerable<TBusinessEntity> entities)
            where TBusinessEntity : class, new()
        {
            try
            {
                using (AppDbContext context = new AppDbContext())
                {
                    context.Set<TBusinessEntity>().RemoveRange(entities);

                    int erc = context.SaveChanges();

                    var result = erc > 0;

                    if (result == true)
                    {
                        // update the generation
                        Cache.NextGeneration<TBusinessEntity>();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ThrowHelper.ReThrow<TBusinessEntity>(ex);
            }
        }

        #endregion
    }
}
