using RefactorName.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.RepositoryInterface.Queries
{
    public class SortOrderEntry
    {
        /// <summary>
        /// Gets the kind of sort order
        /// </summary>
        public SortOrderEnum SortOrder { get; private set; }

        /// <summary>
        /// Gets property name for the property to sort by.
        /// </summary>
        public string SortPropertyName { get; private set; }

        /// <summary>
        /// Initializes new instance of <see cref="SortOrderEntry"/>.
        /// </summary>
        /// <param name="sortOrder">kind of sort order.</param>
        /// <param name="sortPropertyName">property to sort by.</param>
        public SortOrderEntry(SortOrderEnum sortOrder, string sortPropertyName)
        {
            this.SortOrder = sortOrder;
            this.SortPropertyName = sortPropertyName;
        }
    }
}
