using RefactorName.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Caching;
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
        public QueryConstraints(bool withCache)
            : this(withCache, CacheItemPriority.Default, default(int?))
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryConstraints{T}"/> class.
        /// </summary>
        /// <param name="withCache">determines if the <see cref="IQueryResult{T}"/> will be cached or not.</param>
        /// <param name="priority">proiority of <see cref="IQueryResult{T}"/> cache when <see cref="IsWithCache"/> is set true.</param>
        /// <param name="timeoutInSeconds">life time long in second of <see cref="IQueryResult{T}"/> cache when <see cref="IsWithCache"/> is set true.</param>
        public QueryConstraints(bool withCache, CacheItemPriority priority, int? timeoutInSeconds)
        {
            IsWithCache = withCache;
            Priority = priority;
            TimeoutInSeconds = timeoutInSeconds;

            ModelType = typeof(T);
            PageSize = 50;
            PageNumber = 1;
            sortOrder = new List<SortOrderEntry>();
            includePaths = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryConstraints{T}"/> class.
        /// </summary>
        public QueryConstraints()
            : this(false)
        {

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
        public IEnumerable<SortOrderEntry> SortOrder => sortOrder;

        /// <summary>
        /// Gets paths of navigation properties that will be returned with Queries.
        /// </summary>
        public IEnumerable<string> IncludePaths => includePaths;

        /// <summary>
        /// Gets the condition that will be applied on the rows.
        /// </summary>
        public Expression<Func<T, bool>> Predicate { get; private set; }

        /// <summary>
        /// Gets the proiority of <see cref="IQueryResult{T}"/> cache when <see cref="IsWithCache"/> is set true.
        /// </summary>
        public CacheItemPriority Priority { get; private set; }

        /// <summary>
        /// Gets the life time long in second of <see cref="IQueryResult{T}"/> cache when <see cref="IsWithCache"/> is set true.
        /// </summary>
        public int? TimeoutInSeconds { get; private set; }

        /// <summary>
        /// Gets flag determines if the <see cref="IQueryResult{T}"/> will be cached or not.
        /// </summary>
        public bool IsWithCache { get; private set; }

        /// <summary>
        /// Use paging
        /// </summary>
        /// <param name="pageNumber">Page to get (one based index).</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <returns>Current instance</returns>
        public IQueryConstraints<T> Page(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageNumber > 1000)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be between 1 and 1000.");

            if (pageSize < 1 || pageSize > 1000)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be between 1 and 1000.");

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
                throw new ArgumentNullException(nameof(propertyName));

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
                throw new ArgumentNullException(nameof(propertyName));

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
                throw new ArgumentNullException(nameof(propertyName));

            ValidatePropertyName(propertyName);

            sortOrder.Add(new SortOrderEntry(SortOrderEnum.Descending, propertyName));

            return this;
        }

        /// <summary>
        /// Property to sort by (ascending)
        /// </summary>
        /// <param name="property">The property.</param>
        public IQueryConstraints<T> SortBy(Expression<Func<T, object>> property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            MemberExpression expression = property.GetMemberInfo();
            string name = expression.GetName();

            SortBy(name);

            return this;
        }

        /// <summary>
        /// Property to sort by (descending)
        /// </summary>
        /// <param name="property">The property</param>
        public IQueryConstraints<T> SortByDescending(Expression<Func<T, object>> property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            MemberExpression expression = property.GetMemberInfo();
            string name = expression.GetName();

            SortByDescending(name);

            return this;
        }

        /// <summary>
        /// Include navigation property to be included. in returned query.
        /// </summary>
        /// <param name="path">Property path to be included.</param>
        public IQueryConstraints<T> IncludePath(Expression<Func<T, object>> property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            MemberExpression expression = property.GetMemberInfo();
            string name = expression.GetName();

            IncludePath(name);

            return this;
        }

        /// <summary>
        /// Include navigation property to be included in returned query.
        /// </summary>
        /// <param name="path">Property path to be included.</param>
        public IQueryConstraints<T> IncludePath(string propertyName)
        {
            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));

            if (includePaths.Contains(propertyName))
                throw new InvalidOperationException($"The property [{propertyName}] is already included previously for this instance of QueryConstraints");

            ValidatePropertyName(propertyName);

            includePaths.Add(propertyName);

            return this;
        }

        /// <summary>
        /// Apply a filter on rows.
        /// </summary>
        /// <param name="predicate">condition to be applied.</param>
        public IQueryConstraints<T> Where(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            // the following condition prevent the client code from calling the where metohd more than once.
            if (Predicate != null)
                throw new InvalidOperationException("Can not call the [Where] method more than once on the same instance of the QueryConstraints class");

            Predicate = predicate;

            return this;
        }

        /// <summary>
        /// Concatenate old predicate (And) passed predicate.
        /// </summary>
        /// <param name="predicate">condition to be concatenated.</param>
        /// <returns></returns>
        public IQueryConstraints<T> AndAlso(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

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
        public IQueryConstraints<T> OrElse(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            if (Predicate == null)
                return Where(predicate);

            Predicate = Predicate.OrElse(predicate);

            return this;
        }

        /// <summary>
        /// Force the result of this <see cref="IQueryConstraints{T}"/> to be cached.
        /// </summary>
        /// <returns></returns>
        public IQueryConstraints<T> WithCache()
        {
            IsWithCache = true;

            return this;
        }

        /// <summary>
        /// Force the result of this <see cref="IQueryConstraints{T}"/> to be cached.
        /// </summary>
        /// <param name="priority">the proiorty of cached value.</param>
        /// <param name="timeoutInSeconds">life length of cache item, null if it is forever.</param>
        /// <returns></returns>
        public IQueryConstraints<T> WithCache(CacheItemPriority priority, int? timeoutInSeconds)
        {
            IsWithCache = true;
            Priority = priority;
            TimeoutInSeconds = timeoutInSeconds;

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
                throw new ArgumentNullException(nameof(name));

            // the case of simple property
            if (!name.Contains('.'))
            {
                if (ModelType.GetProperty(name) == null)
                    throw new ArgumentException($"'{name}' is not a public property of '{ModelType.FullName}'");
            }
            else // the case of compound property (series of properties)
            {
                string[] parts = name.Split(new char[] { '.' });
                Type tempType = ModelType;

                foreach (string propertyName in parts)
                {
                    PropertyInfo property = tempType.GetProperty(propertyName);

                    if (property == null)
                        throw new ArgumentException($"'{propertyName}' is not a public property of '{tempType.FullName}'");

                    if (!property.PropertyType.IsGenericType)
                        tempType = property.PropertyType;
                    else
                        tempType = property.PropertyType.GetGenericArguments().First();
                }
            }
        }

        public override string ToString()
        {
            string sort = sortOrder.Count > 0 ? " Sort[{0}]" : "";
            string page = " Page[Number:{0}, Size:{1}]";
            string include = includePaths.Count > 0 ? " Include[{0}]" : "";
            string where = Predicate != null ? " Where[{0}]" : "";

            string sortValue = sortOrder.Select(x => x.SortPropertyName + " " + x.SortOrder.ToString())
                .Aggregate((current, next) => current + ", " + next);

            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrEmpty(sort) == false)
                sb.AppendFormat(sort, sortValue);

            if (string.IsNullOrEmpty(page) == false)
                sb.AppendFormat(page, PageNumber, PageSize);

            if (string.IsNullOrEmpty(include) == false)
                sb.AppendFormat(include, includePaths.Aggregate((current, next) => current + ", " + next));

            if (string.IsNullOrEmpty(where) == false)
                sb.AppendFormat(where, Predicate.ToString());

            return sb.ToString();
        }
    }
}
