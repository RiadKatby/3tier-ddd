using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.RepositoryInterface.Queries
{
    /// <summary>
    /// Typed constraints
    /// </summary>
    /// <typeparam name="T">Model to query</typeparam>
    public class QueryConstraints<T> : IQueryConstraints<T> where T : class
    {
        private ICollection<SortOrderEntry> sortOrder;
        private ICollection<string> includePaths;

        /// <summary>
        /// Gets model which will be queried.
        /// </summary>
        protected Type ModelType { get; set; }

        /// <summary>
        /// Gets start record (in the data source)
        /// </summary>
        /// <remarks>Calculated with the help of PageNumber and PageSize.</remarks>
        public int StartRecord
        {
            get
            {
                if (PageNumber <= 1)
                    return 0;
                return (PageNumber - 1) * (PageSize);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryConstraints{T}"/> class.
        /// </summary>
        /// <remarks>Will per default return the first 50 items</remarks>
        public QueryConstraints()
        {
            ModelType = typeof(T);
            PageSize = 50;
            PageNumber = 1;
            sortOrder = new List<SortOrderEntry>();
            includePaths = new List<string>();
        }

        #region IQueryConstraints<T> Members

        /// <summary>
        /// Gets number of items per page (when paging is used)
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// Gets page number (one based index)
        /// </summary>
        public int PageNumber { get; private set; }

        /// <summary>
        /// Gets properties entries for the property to sort by.
        /// </summary>
        public IEnumerable<SortOrderEntry> SortOrder
        {
            get { return sortOrder; }
        }

        /// <summary>
        /// Gets paths of navigation properties that will be returned with Queries.
        /// </summary>
        public IEnumerable<string> IncludePaths
        {
            get { return includePaths; }
        }

        /// <summary>
        /// Gets the condition that will be applied on the rows.
        /// </summary>
        public System.Linq.Expressions.Expression<Func<T, bool>> Predicate { get; private set; }

        /// <summary>
        /// Use paging
        /// </summary>
        /// <param name="pageNumber">Page to get (one based index).</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <returns>Current instance</returns>
        public IQueryConstraints<T> Page(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageNumber > 1000)
                throw new ArgumentOutOfRangeException("pageNumber", "Page number must be between 1 and 1000.");

            if (pageSize < 1 || pageNumber > 1000)
                throw new ArgumentOutOfRangeException("pageSize", "Page size must be between 1 and 1000.");

            PageSize = pageSize;
            PageNumber = pageNumber;

            return this;
        }

        /// <summary>
        /// Sort ascending by a property
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Current instance</returns>
        public IQueryConstraints<T> SortBy(string propertyName)
        {
            if (propertyName == null)
                throw new ArgumentNullException("propertyName");

            ValidatePropertyName(propertyName);

            sortOrder.Add(new SortOrderEntry(SortOrderEnum.Ascending, propertyName));

            return this;
        }

        /// <summary>
        /// Sort by a property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="sortDir">Direction of sorting.</param>
        /// <returns>Current instance</returns>
        public IQueryConstraints<T> SortBy(string propertyName, SortOrderEnum sortDir)
        {
            if (propertyName == null)
                throw new ArgumentNullException("propertyName");

            ValidatePropertyName(propertyName);

            sortOrder.Add(new SortOrderEntry(sortDir, propertyName));

            return this;
        }

        /// <summary>
        /// Sort descending by a property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Current instance</returns>
        public IQueryConstraints<T> SortByDescending(string propertyName)
        {
            if (propertyName == null)
                throw new ArgumentNullException("propertyName");

            ValidatePropertyName(propertyName);

            sortOrder.Add(new SortOrderEntry(SortOrderEnum.Descending, propertyName));

            return this;
        }

        /// <summary>
        /// Property to sort by (ascending)
        /// </summary>
        /// <param name="property">The property.</param>
        public IQueryConstraints<T> SortBy(System.Linq.Expressions.Expression<Func<T, object>> property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            var expression = property.GetMemberInfo();
            var name = expression.GetName();

            SortBy(name);

            return this;
        }

        /// <summary>
        /// Sort by a property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="sortDir">Direction of sorting.</param>
        /// <returns>Current instance</returns>
        //public IQueryConstraints<T> SortBy(string propertyName, SortOrderEnum sortDir)
        //{
        //    if (propertyName == null)
        //        throw new ArgumentNullException("propertyName");

        //    ValidatePropertyName(propertyName);

        //    sortOrder.Add(new SortOrderEntry(sortDir, propertyName));

        //    return this;
        //}

        /// <summary>
        /// Property to sort by (descending)
        /// </summary>
        /// <param name="property">The property</param>
        public IQueryConstraints<T> SortByDescending(System.Linq.Expressions.Expression<Func<T, object>> property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            var expression = property.GetMemberInfo();
            var name = expression.GetName();

            SortByDescending(name);

            return this;
        }

        /// <summary>
        /// Include navigation property to be included in returned query.
        /// </summary>
        /// <param name="path">Property path to be included.</param>
        public IQueryConstraints<T> IncludePath(string propertyName)
        {
            if (propertyName == null)
                throw new ArgumentNullException("propertyName");

            ValidatePropertyName(propertyName);

            includePaths.Add(propertyName);

            return this;
        }

        /// <summary>
        /// Include navigation property to be included. in returned query.
        /// </summary>
        /// <param name="path">Property path to be included.</param>
        public IQueryConstraints<T> IncludePath(System.Linq.Expressions.Expression<Func<T, object>> property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            var expression = property.GetMemberInfo();
            var name = expression.GetName();

            IncludePath(name);

            return this;
        }

        /// <summary>
        /// Apply a filter on rows.
        /// </summary>
        /// <param name="predicate">condition to be applied.</param>
        public IQueryConstraints<T> Where(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            Predicate = predicate;

            return this;
        }

        /// <summary>
        /// Concatenate old predicate (And) passed predicate.
        /// </summary>
        /// <param name="predicate">condition to be concatenated.</param>
        /// <returns></returns>
        public IQueryConstraints<T> AndAlso(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            if (Predicate == null)
                return Where(predicate);

            Predicate = Predicate.AndAlso(predicate);

            return this;
        }

        /// <summary>
        /// Concatenate old predicate (Or) passed predicate.
        /// </summary>
        /// <param name="predicate">condition to be concatenated.</param>
        /// <returns></returns>
        public IQueryConstraints<T> OrElse(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            if (Predicate == null)
                return Where(predicate);

            Predicate = Predicate.OrElse(predicate);

            return this;
        }

        #endregion

        /// <summary>
        /// Make sure that the property exists in the model.
        /// </summary>
        /// <param name="name">The name.</param>
        protected virtual void ValidatePropertyName(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            // the case of simple property
            if (!name.Contains('.'))
            {
                if (ModelType.GetProperty(name) == null)
                    throw new ArgumentException(string.Format("'{0}' is not a public property of '{1}'", name, ModelType.FullName));
            }
            else // the case of compound property (series of properties)
            {
                string[] parts = name.Split(new char[] { '.' });
                Type tempType = ModelType;

                foreach (string propertyName in parts)
                {
                    PropertyInfo property = tempType.GetProperty(propertyName);

                    if (property == null)
                        throw new ArgumentException(string.Format("'{0}' is not a public property of '{1}'", propertyName, tempType.FullName));

                    if (!property.PropertyType.IsGenericType)
                        tempType = property.PropertyType;
                    else
                        tempType = property.PropertyType.GetGenericArguments().First();
                }
            }
        }

    }
}
