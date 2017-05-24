using RefactorName.Core;
using RefactorName.GraphDiff;
using RefactorName.RepositoryInterface;
using RefactorName.RepositoryInterface.Queries;
using RefactorName.SqlServerRepository.Caching;
using RefactorName.SqlServerRepository.Queries;
using System;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace RefactorName.SqlServerRepository
{
    /// <summary>
    /// Represents Generic Repository that retirve data from sql server database.
    /// </summary>
    public class GenericQueryRepository : IGenericQueryRepository
    {
        #region IGenericQueryRepository Members

        /// <summary>
        /// Returns single BusinessObject of table that satisfies the given <paramref name="id"/> or <see cref="EntityNotFoundException"/> will be thrown.
        /// </summary>
        /// <typeparam name="TBusinessEntity">The type of BusinessObject of source table.</typeparam>
        /// <param name="id">Id of the BusinessObject.</param>
        /// <returns>BusinessObject the satisfy the <paramref name="id"/>.</returns>
        /// <exception cref="EntityNotFoundException">When no related BusinessObject is found in table with the given <paramref name="id"/>.</exception>
        /// <exception cref="RepositoryException">When an error on the database level is occurred and couldn't be recovered.</exception>
        public TBusinessEntity Single<TBusinessEntity>(int id)
            where TBusinessEntity : class, new()
        {
            var entity = SingleOrDefault<TBusinessEntity>(id);
            if (entity == null)
                throw new EntityNotFoundException(typeof(TBusinessEntity).Name, id);

            return entity;
        }

        /// <summary>
        /// Returns single BusinessObject of table that satisfies the given <paramref name="id"/> or a null if no such element is found.
        /// </summary>
        /// <typeparam name="TBusinessEntity">The type of BusinessObject of source table.</typeparam>
        /// <param name="id">Id of the BusinessObject.</param>
        /// <returns>BusinessObject the satisfy the <paramref name="id"/>.</returns>
        /// <exception cref="RepositoryException">When an error on the database level is occurred and couldn't be recovered.</exception>
        public TBusinessEntity SingleOrDefault<TBusinessEntity>(int id)
            where TBusinessEntity : class, new()
        {
            try
            {
                using (AppDbContext context = new AppDbContext())
                    return context.Set<TBusinessEntity>().Find(id);
            }
            catch (Exception ex)
            {
                throw ThrowHelper.ReThrow<TBusinessEntity>(ex);
            }
        }

        /// <summary>
        /// Returns full graph of single BusinessObject of table that satisfies the given <paramref name="id"/> or <see cref="EntityNotFoundException"/> will be thrown.
        /// </summary>
        /// <typeparam name="TBusinessEntity">The type of BusinessObject of source table.</typeparam>
        /// <param name="id">Id of the BusinessObject.</param>
        /// <returns>BusinessObject the satisfy the <paramref name="id"/>.</returns>
        /// <exception cref="EntityNotFoundException">When no related businessObject is found with the given <paramref name="id"/>.</exception>
        /// <exception cref="RepositoryException">When an error on the database level is occurred and couldn't be recovered.</exception>
        public TBusinessEntity SingleWithGraph<TBusinessEntity>(int id)
            where TBusinessEntity : class, new()
        {
            var entity = SingleOrDefaultWithGraph<TBusinessEntity>(id);
            if (entity == null)
                throw new EntityNotFoundException(typeof(TBusinessEntity).Name, id);

            return entity;
        }

        /// <summary>
        /// Returns full graph of single BusinessObject of table that satisfies the given <paramref name="id"/> or null if no such element is found.
        /// </summary>
        /// <typeparam name="TBusinessEntity">The type of BusinessObject of source table.</typeparam>
        /// <param name="id">Id of the BusinessObject.</param>
        /// <returns>BusinessObject the satisfy the <paramref name="id"/>.</returns>
        /// <exception cref="RepositoryException">When an error on the database level is occurred and couldn't be recovered.</exception>
        public TBusinessEntity SingleOrDefaultWithGraph<TBusinessEntity>(int id)
            where TBusinessEntity : class, new()
        {
            try
            {
                using (AppDbContext context = new AppDbContext())
                {
                    var primaryKeyPredicate = CreatePredicate<TBusinessEntity>(context, id);

                    return context.LoadAggregate(primaryKeyPredicate);
                }
            }
            catch (Exception ex)
            {
                throw ThrowHelper.ReThrow<TBusinessEntity>(ex);
            }
        }

        /// <summary>
        /// Returns custom graph of single BusinessObject of table that satisfies the given <see cref="IQueryConstraints{T}"/> or <see cref="EntityNotFoundException"/> will be thrown.
        /// </summary>
        /// <typeparam name="TBusinessEntity">The type of BusinessObject of source table.</typeparam>
        /// <param name="constraints">QueryConstraints object to be satisfied.</param>
        /// <returns>BusinessObject the satisfy the <paramref name="constraints"/>.</returns>
        /// <exception cref="EntityNotFoundException">When no related businessObject is found with the given <paramref name="constraints"/>.</exception>
        /// <exception cref="RepositoryException">When an error on the database level is occurred and couldn't be recovered.</exception>
        public TBusinessEntity Single<TBusinessEntity>(IQueryConstraints<TBusinessEntity> constraints)
            where TBusinessEntity : class, new()
        {
            var entity = SingleOrDefault<TBusinessEntity>(constraints);
            if (entity == null)
                throw new EntityNotFoundException(typeof(TBusinessEntity).Name, -1);

            return entity;
        }

        /// <summary>
        /// Returns custom graph of single BusinessObject of table that satisfies a specified <see cref="IQueryConstraints{T}"/> or null if no such element is found.
        /// </summary>
        /// <typeparam name="TBusinessEntity">The type of BusinessObject of source table.</typeparam>
        /// <param name="constraints">QueryConstraints object to be satisfied.</param>
        /// <returns>BusinessObject the satisfy the <paramref name="constraints"/>.</returns>
        /// <exception cref="RepositoryException">When an error on the database level is occurred and couldn't be recovered.</exception>
        public TBusinessEntity SingleOrDefault<TBusinessEntity>(IQueryConstraints<TBusinessEntity> constraints)
            where TBusinessEntity : class, new()
        {
            try
            {
                using (AppDbContext context = new AppDbContext())
                    return context.Set<TBusinessEntity>()
                        .ToSearchResult<TBusinessEntity>(constraints)
                        .Items
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ThrowHelper.ReThrow<TBusinessEntity>(ex);
            }
        }

        /// <summary>
        /// Returns record count of table.
        /// </summary>
        /// <typeparam name="TBusinessEntity">The type of BusinessObject of source table.</typeparam>
        /// <returns>The number of elements in the table.</returns>
        /// <exception cref="RepositoryException">When an error on the database level is occurred and couldn't be recovered.</exception>
        public int GetCount<TBusinessEntity>()
            where TBusinessEntity : class, new()
        {
            try
            {
                using (AppDbContext context = new AppDbContext())
                    return context.Set<TBusinessEntity>()
                                  .Count();
            }
            catch (Exception ex)
            {
                throw ThrowHelper.ReThrow<TBusinessEntity>(ex);
            }
        }

        /// <summary>
        /// Returns record count of table that satisfy the given <paramref name="constraints"/>.
        /// </summary>
        /// <typeparam name="TBusinessEntity">The type of BusinessObject of source table.</typeparam>
        /// <param name="constraints">QueryConstraints object to be satisfied.</param>
        /// <returns>The number of elements in the table that satisfies the given <paramref name="constraints"/>.</returns>
        /// <exception cref="RepositoryException">When an error on the database level is occurred and couldn't be recovered.</exception>
        public int GetCount<TBusinessEntity>(IQueryConstraints<TBusinessEntity> constraints)
            where TBusinessEntity : class, new()
        {
            try
            {
                using (AppDbContext context = new AppDbContext())
                    return context.Set<TBusinessEntity>()
                        .Count(constraints.Predicate);
            }
            catch (Exception ex)
            {
                throw ThrowHelper.ReThrow<TBusinessEntity>(ex);
            }
        }

        /// <summary>
        /// Returns <see cref="IQueryResult{T}"/> that satisfy the given <paramref name="constraints"/> on the table.
        /// </summary>
        /// <typeparam name="TBusinessEntity">The type of BusinessObject of source table.</typeparam>
        /// <param name="constraints">QueryConstraints object to be satisfied.</param>
        /// <returns>The result records that satisfy the given <paramref name="constraints"/>.</returns>
        /// <exception cref="RepositoryException">When an error on the database level is occurred and couldn't be recovered.</exception>
        public IQueryResult<TBusinessEntity> Find<TBusinessEntity>(IQueryConstraints<TBusinessEntity> constraints)
            where TBusinessEntity : class, new()
        {
            try
            {
                using (AppDbContext context = new AppDbContext())
                {
                    IQueryResult<TBusinessEntity> results = null;

                    // get the appropriate cache key for this query
                    string key = Cache.CacheKey(constraints);

                    // check if it's in the cache already
                    if (!Cache.Provider.TryGet(key, out results))
                    {
                        Debug.WriteLine($"NOT FOUND cache for: {key}");

                        // if not, then run the query
                        results = context.Set<TBusinessEntity>().ToSearchResult(constraints);

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
            }
            catch (Exception ex)
            {
                throw ThrowHelper.ReThrow<TBusinessEntity>(ex);
            }
        }

        #endregion

        private Expression<Func<TBusinessEntity, bool>> CreatePredicate<TBusinessEntity>(AppDbContext context, int id)
        {
            var entityType = typeof(TBusinessEntity);

            var metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace
                    .GetItems<EntityType>(DataSpace.OSpace)
                    .SingleOrDefault(p => p.FullName == entityType.FullName);

            if (metadata == null)
            {
                throw new InvalidOperationException(String.Format("The type {0} is not known to the DbContext.", entityType.FullName));
            }

            var keyProperties = metadata.KeyMembers
                .Select(k => entityType.GetProperty(k.Name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                .ToList();

            ParameterExpression parameter = Expression.Parameter(entityType);
            Expression expression = CreateEqualsExpression(id, keyProperties[0], parameter);
            //for (int i = 1; i < keyProperties.Count; i++)
            //{
            //    expression = Expression.And(expression, CreateEqualsExpression(entity, keyProperties[i], parameter));
            //}

            return Expression.Lambda<Func<TBusinessEntity, bool>>(expression, parameter);
        }

        private static Expression CreateEqualsExpression(object entityId, PropertyInfo keyProperty, Expression parameter)
        {
            return Expression.Equal(Expression.Property(parameter, keyProperty), Expression.Constant(entityId, keyProperty.PropertyType));
        }
    }
}
