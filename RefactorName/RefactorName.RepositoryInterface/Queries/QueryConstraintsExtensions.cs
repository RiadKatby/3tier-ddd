using RefactorName.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.RepositoryInterface.Queries
{
    public static class QueryConstraintsExtensions
    {
        public static IQueryConstraints<T> WhereIf<T>(this IQueryConstraints<T> constraints, Expression<Func<T, bool>> predicate, bool condition)
           where T : class
        {
            if (condition)
                constraints.Where(predicate);

            return constraints;
        }

        public static IQueryConstraints<T> AndAlsoIf<T>(this IQueryConstraints<T> constraints, Expression<Func<T, bool>> predicate, bool condition)
            where T : class
        {
            if (condition)
                constraints.AndAlso(predicate);

            return constraints;
        }

        public static IQueryConstraints<T> OrElseIf<T>(this IQueryConstraints<T> constraints, Expression<Func<T, bool>> predicate, bool condition)
            where T : class
        {
            if (condition)
                constraints.OrElse(predicate);

            return constraints;
        }

        public static IQueryConstraints<T> PageAndSort<T>(this IQueryConstraints<T> constraints, SearchCriteria criteria, Expression<Func<T, object>> defaultSortExpression)
            where T : class
        {
            constraints.Page(criteria.PageNumber, criteria.PageSize);

            if (!string.IsNullOrEmpty(criteria.Sort))
                constraints.SortBy(criteria.Sort, criteria.SortDirection);
            else
                constraints.SortByDescending(defaultSortExpression);

            return constraints;
        }
    }
}
