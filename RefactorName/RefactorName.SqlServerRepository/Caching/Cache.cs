using RefactorName.RepositoryInterface;
using RefactorName.RepositoryInterface.Queries;
using RefactorName.SqlServerRepository.Caching.Hash;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.SqlServerRepository.Caching
{
    public static class Cache
    {
        public static ICachingProvider Provider { get; private set; }

        static Cache()
        {
            // TODO: this must be changed into factory depending on Config File.
            Provider = RepositoryFactory.CreateCacheProvider();
        }

        internal static string CacheKey<TBusinessEntity>(object id)
        {
            return string.Concat(typeof(TBusinessEntity).Name, "/", id);
        }

        internal static string CacheKey<TBusinessEntity>(IQueryConstraints<TBusinessEntity> constraints)
            where TBusinessEntity : class
        {
            Expression predicate = constraints.Predicate;

            string pagePart = $"Page[{constraints.PageNumber}, {constraints.PageSize}]";

            string includePart = constraints.IncludePaths.Count() > 0
                ? $"Include[{constraints.IncludePaths.Aggregate((c, n) => c + ", " + n)}]"
                : "";

            string sortPart = constraints.SortOrder.Count() > 0
                ? $"Sort[{constraints.SortOrder.Select(x => x.SortPropertyName + " " + x.SortOrder.ToString()).Aggregate((c, n) => c + ", " + n)}]"
                : "";

            StringBuilder key = new StringBuilder();
            key.Append(pagePart);

            if (string.IsNullOrEmpty(includePart) == false)
                key.Append(", " + includePart);

            if (string.IsNullOrEmpty(sortPart) == false)
                key.Append(", " + sortPart);

            if (constraints.Predicate != null)
                key.Append(", " + $"Where[{GetHash(predicate)}]");

            return string.Concat(typeof(TBusinessEntity).Name, "/", GetGeneration<TBusinessEntity>(), "/", key);
        }

        private static int GetGeneration<TBusinessEntity>()
        {
            int generation;
            return !Provider.TryGet(GetGenerationKey<TBusinessEntity>(), out generation) ? 1 : generation;
        }

        public static int NextGeneration<TBusinessEntity>()
        {
            return Provider.Increment(GetGenerationKey<TBusinessEntity>(), 1, 1);
        }

        private static string GetGenerationKey<TBusinessEntity>()
        {
            return String.Format("{0}/Generation", typeof(TBusinessEntity).Name);
        }

        // special thanks to Pete Montgomery's post here: http://petemontgomery.wordpress.com/2008/08/07/caching-the-results-of-linq-queries/
        private static string GetHash(Expression expression)
        {
            if (expression == null)
                return null;

            // locally evaluate as much of the query as possible
            expression = Evaluator.PartialEval(expression);

            // support local collections
            expression = LocalCollectionExpander.Rewrite(expression);

            // use the string representation of the expression for the cache key
            return expression.ToString();
        }

        #region Figure out Primary Key
        private static Dictionary<Type, List<PropertyInfo>> primaryKeys = new Dictionary<Type, List<PropertyInfo>>();

        private static IEnumerable<PropertyInfo> GetPrimaryKeyFieldsFor<TBusinessEntity>()
        {
            Type entityType = typeof(TBusinessEntity);

            if (primaryKeys.ContainsKey(entityType))
                return primaryKeys[entityType];


            AppDbContext dbContext = new AppDbContext();
            var _objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;

            var metadata = _objectContext.MetadataWorkspace
                    .GetItems<EntityType>(DataSpace.OSpace)
                    .SingleOrDefault(p => p.FullName == entityType.FullName);

            if (metadata == null)
            {
                throw new InvalidOperationException(String.Format("The type {0} is not known to the DbContext.", entityType.FullName));
            }

            var results = metadata.KeyMembers
                .Select(k => entityType.GetProperty(k.Name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                .ToList();

            primaryKeys[entityType] = results;

            return results;
        }

        /// <summary>
        /// Retrive the Primary Key value of the specifed entity.
        /// </summary>
        /// <param name="entity">Business Entity to retrive the primary key of.</param>
        internal static object GetPrimaryKeyValue<TBusinessEntity>(TBusinessEntity entity)
        {
            var primaryKeyProperty = GetPrimaryKeyFieldsFor<TBusinessEntity>().FirstOrDefault();
            return primaryKeyProperty.GetValue(entity);
        }
        #endregion
    }
}
