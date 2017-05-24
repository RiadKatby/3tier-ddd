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
    /// Represents repository that retrieves Business Entities generically.
    /// </summary>
    public interface IGenericQueryRepository
    {
        /// <summary>
        /// Returns single BusinessObject of table that satisfies the given <paramref name="id"/> or <see cref="EntityNotFoundException"/> will be thrown.
        /// </summary>
        /// <typeparam name="TBusinessEntity">The type of BusinessObject of source table.</typeparam>
        /// <param name="id">Id of the BusinessObject.</param>
        /// <returns>BusinessObject the satisfy the <paramref name="id"/>.</returns>
        /// <exception cref="EntityNotFoundException">When no related BusinessObject is found in table with the given <paramref name="id"/>.</exception>
        /// <exception cref="RepositoryException">When an error on the database level is occurred and couldn't be recovered.</exception>
        TBusinessEntity Single<TBusinessEntity>(int id) where TBusinessEntity : class, new();

        /// <summary>
        /// Returns single BusinessObject of table that satisfies the given <paramref name="id"/> or a null if no such element is found.
        /// </summary>
        /// <typeparam name="TBusinessEntity">The type of BusinessObject of source table.</typeparam>
        /// <param name="id">Id of the BusinessObject.</param>
        /// <returns>BusinessObject the satisfy the <paramref name="id"/>.</returns>
        /// <exception cref="RepositoryException">When an error on the database level is occurred and couldn't be recovered.</exception>
        TBusinessEntity SingleOrDefault<TBusinessEntity>(int id) where TBusinessEntity : class, new();

        /// <summary>
        /// Returns full graph of single BusinessObject of table that satisfies the given <paramref name="id"/> or <see cref="EntityNotFoundException"/> will be thrown.
        /// </summary>
        /// <typeparam name="TBusinessEntity">The type of BusinessObject of source table.</typeparam>
        /// <param name="id">Id of the BusinessObject.</param>
        /// <returns>BusinessObject the satisfy the <paramref name="id"/>.</returns>
        /// <exception cref="EntityNotFoundException">When no related businessObject is found with the given <paramref name="id"/>.</exception>
        /// <exception cref="RepositoryException">When an error on the database level is occurred and couldn't be recovered.</exception>
        TBusinessEntity SingleWithGraph<TBusinessEntity>(int id) where TBusinessEntity : class, new();

        /// <summary>
        /// Returns full graph of single BusinessObject of table that satisfies the given <paramref name="id"/> or null if no such element is found.
        /// </summary>
        /// <typeparam name="TBusinessEntity">The type of BusinessObject of source table.</typeparam>
        /// <param name="id">Id of the BusinessObject.</param>
        /// <returns>BusinessObject the satisfy the <paramref name="id"/>.</returns>
        /// <exception cref="RepositoryException">When an error on the database level is occurred and couldn't be recovered.</exception>
        TBusinessEntity SingleOrDefaultWithGraph<TBusinessEntity>(int id) where TBusinessEntity : class, new();

        /// <summary>
        /// Returns custom graph of single BusinessObject of table that satisfies the given <see cref="IQueryConstraints{T}"/> or <see cref="EntityNotFoundException"/> will be thrown.
        /// </summary>
        /// <typeparam name="TBusinessEntity">The type of BusinessObject of source table.</typeparam>
        /// <param name="constraints">QueryConstraints object to be satisfied.</param>
        /// <returns>BusinessObject the satisfy the <paramref name="constraints"/>.</returns>
        /// <exception cref="EntityNotFoundException">When no related businessObject is found with the given <paramref name="constraints"/>.</exception>
        /// <exception cref="RepositoryException">When an error on the database level is occurred and couldn't be recovered.</exception>
        TBusinessEntity Single<TBusinessEntity>(IQueryConstraints<TBusinessEntity> constraints) where TBusinessEntity : class, new();

        /// <summary>
        /// Returns custom graph of single BusinessObject of table that satisfies a specified <see cref="IQueryConstraints{T}"/> or null if no such element is found.
        /// </summary>
        /// <typeparam name="TBusinessEntity">The type of BusinessObject of source table.</typeparam>
        /// <param name="constraints">QueryConstraints object to be satisfied.</param>
        /// <returns>BusinessObject the satisfy the <paramref name="constraints"/>.</returns>
        /// <exception cref="RepositoryException">When an error on the database level is occurred and couldn't be recovered.</exception>
        TBusinessEntity SingleOrDefault<TBusinessEntity>(IQueryConstraints<TBusinessEntity> constraints) where TBusinessEntity : class, new();

        /// <summary>
        /// Returns record count of table.
        /// </summary>
        /// <typeparam name="TBusinessEntity">The type of BusinessObject of source table.</typeparam>
        /// <returns>The number of elements in the table.</returns>
        /// <exception cref="RepositoryException">When an error on the database level is occurred and couldn't be recovered.</exception>
        int GetCount<TBusinessEntity>() where TBusinessEntity : class, new();

        /// <summary>
        /// Returns record count of table that satisfy the given <paramref name="constraints"/>.
        /// </summary>
        /// <typeparam name="TBusinessEntity">The type of BusinessObject of source table.</typeparam>
        /// <param name="constraints">QueryConstraints object to be satisfied.</param>
        /// <returns>The number of elements in the table that satisfies the given <paramref name="constraints"/>.</returns>
        /// <exception cref="RepositoryException">When an error on the database level is occurred and couldn't be recovered.</exception>
        int GetCount<TBusinessEntity>(IQueryConstraints<TBusinessEntity> constraints) where TBusinessEntity : class, new();

        /// <summary>
        /// Returns <see cref="IQueryResult{T}"/> that satisfy the given <paramref name="constraints"/> on the table.
        /// </summary>
        /// <typeparam name="TBusinessEntity">The type of BusinessObject of source table.</typeparam>
        /// <param name="constraints">QueryConstraints object to be satisfied.</param>
        /// <returns>The result records that satisfy the given <paramref name="constraints"/>.</returns>
        /// <exception cref="RepositoryException">When an error on the database level is occurred and couldn't be recovered.</exception>
        IQueryResult<TBusinessEntity> Find<TBusinessEntity>(IQueryConstraints<TBusinessEntity> constraints) where TBusinessEntity : class, new();
    }
}
