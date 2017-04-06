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
    /// <summary>
    /// Represents repository that retirve Business Entities genericaly.
    /// </summary>
    public interface IGenericQueryRepository
    {
        /// <summary>
        /// Returns single BusinessObject of table that satisfies a specified condition or a null if no such element is found.
        /// </summary>
        /// <typeparam name="TBusinessEntity">The type of BusinessObject of source table.</typeparam>
        /// <param name="id">Id of the BusinessObject.</param>
        /// <returns>The BusinessObject the satisfy the predicate.</returns>
        TBusinessEntity Single<TBusinessEntity>(int id) where TBusinessEntity : class, new();

        /// <summary>
        /// Return one entity which complies to constraints, if more than one entity complies to constraints an exception will be thrown.
        /// </summary>
        /// <typeparam name="TBusinessEntity"></typeparam>
        /// <param name="constraints"></param>
        /// <returns></returns>
        TBusinessEntity Single<TBusinessEntity>(IQueryConstraints<TBusinessEntity> constraints) where TBusinessEntity : class, new();

        /// <summary>
        /// Return one or first entity which complies to constraint, if no or more than one entity complies to constraint the first one or null will be returned.
        /// </summary>
        /// <typeparam name="TBusinessEntity"></typeparam>
        /// <param name="constraints"></param>
        /// <returns></returns>
        TBusinessEntity SingleOrDefault<TBusinessEntity>(IQueryConstraints<TBusinessEntity> constraints) where TBusinessEntity : class, new();

        /// <summary>
        /// Returns the number of elements of the specified T BusinessEntity that satisfies a condition.
        /// </summary>
        /// <typeparam name="TBusinessEntity">The type of BusinessObject of source table.</typeparam>
        /// <param name="constraints">A function to test each element for a condition.</param>
        /// <returns>The number of elements in the sequence that satisfies the condition in the predicate function.</returns>
        int GetCount<TBusinessEntity>(IQueryConstraints<TBusinessEntity> constraints) where TBusinessEntity : class, new();

        /// <summary>
        /// Returns the number of elements in a BusinessObject's table.
        /// </summary>
        /// <typeparam name="TBusinessEntity">The type of BusinessObject of source table.</typeparam>
        /// <returns>The number of elements in the input sequence.</returns>
        int GetCount<TBusinessEntity>() where TBusinessEntity : class, new();

        /// <summary>
        /// Apply QueryConstraints on corresponding table of TEntity type and return the result.
        /// </summary>
        /// <typeparam name="TBusinessEntity">An Entity type to retrieves the entity objects from.</typeparam>
        /// <param name="constraints">An QueryConstraints that has the criteria of search results.</param>
        /// <returns>An object of <see cref="IQueryResult"/> with TEntity objects if any satisfied the QueryConstraints object.</returns>
        IQueryResult<TBusinessEntity> Find<TBusinessEntity>(IQueryConstraints<TBusinessEntity> constraints) where TBusinessEntity : class, new();
    }
}
