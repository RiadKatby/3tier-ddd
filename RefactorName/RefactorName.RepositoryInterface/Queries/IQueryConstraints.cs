using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.RepositoryInterface.Queries
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Entity type.</typeparam>
    public interface IQueryConstraints<T> where T : class
    {
        /// <summary>
        /// Gets number of items per page (when paging is used)
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// Gets page number (one based index)
        /// </summary>
        int PageNumber { get; }

        /// <summary>
        /// Get sort order and kind of that order on properties.
        /// </summary>
        IEnumerable<SortOrderEntry> SortOrder { get; }

        /// <summary>
        /// Gets paths of navigation properties that will be returned with Queries.
        /// </summary>
        IEnumerable<string> IncludePaths { get; }

        /// <summary>
        /// Use paging
        /// </summary>
        /// <param name="pageNumber">Page to get (one based index).</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <returns>Current instance</returns>
        IQueryConstraints<T> Page(int pageNumber, int pageSize);

        /// <summary>
        /// Sort ascending by a property
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Current instance</returns>
        IQueryConstraints<T> SortBy(string propertyName);

        /// <summary>
        /// Sort descending by a property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Current instance</returns>
        IQueryConstraints<T> SortByDescending(string propertyName);

        /// <summary>
        /// Property to sort by (ascending)
        /// </summary>
        /// <param name="property">The property.</param>
        IQueryConstraints<T> SortBy(Expression<Func<T, object>> property);

        /// <summary>
        /// Property to sort by (descending)
        /// </summary>
        /// <param name="property">The property</param>
        IQueryConstraints<T> SortByDescending(Expression<Func<T, object>> property);

        /// <summary>
        /// Include navigation property to be included in returned query.
        /// </summary>
        /// <param name="path">Property path to be included.</param>
        IQueryConstraints<T> IncludePath(string path);

        /// <summary>
        /// Include navigation property to be included. in returned query.
        /// </summary>
        /// <param name="path">Property path to be included.</param>
        IQueryConstraints<T> IncludePath(Expression<Func<T, object>> path);

        /// <summary>
        /// Gets the condition that will be applied on the rows.
        /// </summary>
        System.Linq.Expressions.Expression<Func<T, bool>> Predicate { get; }

        /// <summary>
        /// Apply a filter on rows.
        /// </summary>
        /// <param name="predicate">condition to be applied.</param>
        IQueryConstraints<T> Where(System.Linq.Expressions.Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Concatenate old predicate (And) passed predicate.
        /// </summary>
        /// <param name="predicate">condition to be concatenated.</param>
        /// <returns></returns>
        IQueryConstraints<T> AndAlso(System.Linq.Expressions.Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Concatenate old predicate (Or) passed predicate.
        /// </summary>
        /// <param name="predicate">condition to be concatenated.</param>
        /// <returns></returns>
        IQueryConstraints<T> OrElse(System.Linq.Expressions.Expression<Func<T, bool>> predicate);
    }
}
