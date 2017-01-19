using RefactorName.GraphDiff;
using RefactorName.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.SqlServerRepository
{
    public class GenericRepository : IGenericRepository
    {
        private MyDbContext notifierContext;

        public GenericRepository() { }

        public GenericRepository(MyDbContext context)
        {
            this.notifierContext = context;
        }

        #region IGenericRepository Members

        public virtual TBizEntity Create<TBizEntity>(TBizEntity entity) where TBizEntity : class, new()
        {
            try
            {
                MyDbContext context = notifierContext ?? new MyDbContext();

                TBizEntity updatedEntity = context.UpdateGraph<TBizEntity>(entity);

                if (notifierContext == null)
                {
                    int erc = context.SaveChanges();

                    context.Dispose();
                    return erc > 0 ? updatedEntity : null;
                }

                // else the management of the context is being done by the UnitOfWork

                return updatedEntity;
            }
            catch (Exception ex)
            {
                throw ThrowHelper.ReThrow(ex);
            }
        }

        public virtual TBizEntity Update<TBizEntity>(TBizEntity entity) where TBizEntity : class, new()
        {
            try
            {
                MyDbContext context = notifierContext ?? new MyDbContext();

                TBizEntity updatedEntity = context.UpdateGraph<TBizEntity>(entity);

                if (notifierContext == null)
                {
                    int erc = context.SaveChanges();
                    context.Dispose();

                    return erc > 0 ? updatedEntity : null;
                }
                // else the management of the context is being done by the UnitOfWork

                return updatedEntity;
            }
            catch (Exception ex)
            {
                throw ThrowHelper.ReThrow(ex);
            }
        }

        public virtual bool Delete<T>(T entity) where T : class, new()
        {
            int erc = 0;

            try
            {
                MyDbContext context = notifierContext ?? new MyDbContext();

                context.Set<T>().Attach(entity);
                context.Set<T>().Remove(entity);

                if (notifierContext == null)
                {
                    erc = context.SaveChanges();
                    context.Dispose();
                }

                // else the management of the context is being done by the UnitOfWork

                return erc > 0;
            }
            catch (Exception ex)
            {
                throw ThrowHelper.ReThrow(ex);
            }
        }

        public bool Delete<T>(IEnumerable<T> entities) where T : class, new()
        {
            int erc = 0;

            try
            {
                MyDbContext context = notifierContext ?? new MyDbContext();
                context.Set<T>().RemoveRange(entities);
                if (notifierContext == null)
                {
                    erc = context.SaveChanges();
                    context.Dispose();
                }

                // else the management of the context is being done by the UnitOfWork

                return erc > 0;
            }
            catch (Exception ex)
            {
                throw ThrowHelper.ReThrow(ex);
            }
        }

        #endregion
    }
}
