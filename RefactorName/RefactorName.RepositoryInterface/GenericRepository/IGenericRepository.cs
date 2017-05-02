using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.RepositoryInterface
{
    /// <summary>
    /// Provides generic API to Create, Update and Delete operation for Business Entity in the corresponding database tables.
    /// </summary>
    public interface IGenericRepository
    {
        /// <summary>
        /// Creates (insert) an entity of the Generic Parameter in the corresponding table in the database.
        /// </summary>
        /// <typeparam name="TEntity">An Entity type to insert the entity object in.</typeparam>
        /// <param name="entity">An entity object to be inserted in the table corresponding to Entity type.</param>
        /// <returns>New entity after insert it in the database.</returns>
        TEntity Create<TEntity>(TEntity entity) where TEntity : class, new();

        /// <summary>
        /// Update an entity of Generic Parameter in the corresponding table in the database.
        /// </summary>
        /// <typeparam name="TEntity">An Entity type to insert the entity object in.</typeparam>
        /// <param name="entity">An entity object to be updated in the table corresponding to Entity type.</param>
        /// <returns>New entity object after update it in the database.</returns>
        TEntity Update<TEntity>(TEntity entity) where TEntity : class, new();

        /// <summary>
        /// Delete an entity of Generic Parameter from the corresponding table in the database.
        /// </summary>
        /// <typeparam name="TEntity">An Entity type to delete the entity object from.</typeparam>
        /// <param name="entity">An entity object to be deleted from the table corresponding to Entity type.</param>
        /// <returns>True, if object deleted successfully. False, otherwise.</returns>
        bool Delete<TEntity>(TEntity entity) where TEntity : class, new();

        /// <summary>
        /// Delete entities of Generic Parameter from the corresponding table in the database.
        /// </summary>
        /// <typeparam name="TEntity">An Entity type to delete the entity object from.</typeparam>
        /// <param name="entities">An entities objects to be deleted from the table corresponding to Entity type.</param>
        /// <returns>True, if object deleted successfully. False, otherwise.</returns>
        bool Delete<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, new();
    }
}
