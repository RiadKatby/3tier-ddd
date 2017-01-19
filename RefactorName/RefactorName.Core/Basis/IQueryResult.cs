using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core
{
    /// <summary>
    /// Defines minimum parameters for query search results.
    /// </summary>
    /// <typeparam name="T">Type of return model</typeparam>
    public interface IQueryResult<out T> where T : class
    {
        /// <summary>
        /// Gets all matching items
        /// </summary>
        IEnumerable<T> Items { get; }

        /// <summary>
        /// Gets total number of items (useful when paging is used)
        /// </summary>
        int TotalCount { get; }

        /// <summary>
        /// Gets a nubmer represents the current set page number (when paging used)
        /// </summary>
        int PageNumber { get; }

        /// <summary>
        /// Gets a nubmer represents the page size (when paging used)
        /// </summary>
        int PageSize { get; }
    }
}
