using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core
{
    /// <summary>
    /// Represent the base criteria parameters that are used in Searching methods.
    /// </summary>
    public class SearchCriteria
    {
        /// <summary>
        /// The size of page that is intended to be retrieved.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// The number of page to be retrieved.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// The property name to sort with
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// The sort direction
        /// </summary>
        public string SortDirection { get; set; }

        public SearchCriteria()
        {
            PageSize = 10;
            PageNumber = 1;
        }
    }
}
