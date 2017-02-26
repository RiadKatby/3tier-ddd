using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using RefactorName.RepositoryInterface.Queries;
using RefactorName.Core;

namespace RefactorName.RepositoryInterface
{
    public interface IGenericQueryRepository
    {
        TEntity Single<TEntity>(int id) where TEntity : class;

        /// <summary>
        /// Return one entity which complies to constraints, if more than one entity complies to constraints an exception will be thrown.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="constraints"></param>
        /// <returns></returns>
        TEntity Single<TEntity>(IQueryConstraints<TEntity> constraints) where TEntity : class;

        /// <summary>
        /// Return one or first entity which complies to constraint, if no or more than one entity complies to constraint the first one or null will be returned.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="constraints"></param>
        /// <returns></returns>
        TEntity SingleOrDefault<TEntity>(IQueryConstraints<TEntity> constraints) where TEntity : class;

        int GetCount<TEntity>(IQueryConstraints<TEntity> constraints) where TEntity : class;

        int GetCount<TEntity>() where TEntity : class;

        /// <summary>
        /// Apply QueryConstraints on corresponding table of TEntity type and return the result.
        /// </summary>
        /// <typeparam name="TEntity">An Entity type to retrieves the entity objects from.</typeparam>
        /// <param name="constraints">An QueryConstraints that has the criteria of search results.</param>
        /// <returns>An object of <see cref="IQueryResult"/> with TEntity objects if any satisfied the QueryConstraints object.</returns>
        IQueryResult<TEntity> Find<TEntity>(IQueryConstraints<TEntity> constraints) where TEntity : class;
    }
}
