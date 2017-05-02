using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core
{
    /// <summary>
    /// Search result implementation
    /// </summary>
    /// <typeparam name="T">Model type (i.e. denormalized row)</typeparam>
    public class QueryResult<T> : IQueryResult<T> where T : class
    {
        /// <summary>
        /// Gets all matching items
        /// </summary>
        public IEnumerable<T> Items { get; private set; }

        /// <summary>
        /// Gets total number of items (useful when paging is used, otherwise 0)
        /// </summary>
        public int TotalCount { get; private set; }

        /// <summary>
        /// Gets current page nubmer used to get items (useful when paging is used)
        /// </summary>
        public int PageNumber { get; private set; }

        /// <summary>
        /// Get page size used to get items (useful when paging is used)
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryResult{T}" /> class.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="totalCount">The total count (if paging is used, otherwise <c>0</c>).</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public QueryResult(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            if (totalCount < 0 || totalCount > int.MaxValue)
                throw new ArgumentOutOfRangeException("totalCount", totalCount, "Incorrect value.");

            if (pageNumber < 1 || pageNumber > int.MaxValue)
                throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "Incorrect value.");

            if (pageSize < 1 || pageSize > int.MaxValue)
                throw new ArgumentOutOfRangeException("pageSize", pageSize, "Incorrect value.");

            Items = items;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
