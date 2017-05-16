using RefactorName.WebApp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace RefactorName.WebApp
{
    public static class Extensions
    {
        #region EF Extensions
        ///// <summary>
        /////Method for dynamicly sorting then paging any entity framework IEnumerable result 
        /////Remember, pass only results as IQueryable not as list or IEnumerable to get best practice performance.
        /////e.g) pass db.Users not db.Users.ToList() to ensure execute no more than one select statement.
        ///// </summary>
        ///// <typeparam name="TEntity">any entity type</typeparam>
        ///// <param name="obj">IQueryable result</param>
        ///// <param name="sort">column to sort by</param>
        ///// <param name="sortdir">sorting direction asc or desc</param>
        ///// <param name="page">page index statrting from 1</param>
        ///// <param name="pageSize">rows per page</param>
        ///// <returns>WebGridList{List,Count,pageSize} good for WebGrid.</returns>
        //public static Models.WebGridList<TEntity> PagingAndSorting<TEntity>(this IQueryable<TEntity> source, string sort = "", string sortdir = "desc", int? page = 1, int pageSize = 10)
        //{
        //    int totalCount = 0;
        //    page = page ?? 1;
        //    int actualIndex = Math.Abs((page.Value - 1) * pageSize);
        //    try
        //    {
        //        totalCount = source.Count();
        //        IQueryable<TEntity> list = null;
        //        Type typ = typeof(TEntity);

        //        string direction = String.IsNullOrWhiteSpace(sortdir) ? "OrderByDescending" : sortdir.ToLower() == "asc" ? "OrderBy" : "OrderByDescending";
        //        sort = String.IsNullOrWhiteSpace(sort) ? typ.GetProperties().FirstOrDefault().Name : sort;

        //        string[] propArray = sort.Split('.');


        //        ParameterExpression parameterExpression = Expression.Parameter(typ, "p");
        //        Expression seedExpression = parameterExpression;

        //        Expression aggregateExpression = sort.Split('.').Aggregate(seedExpression, Expression.Property);

        //        MemberExpression memberExpression = aggregateExpression as MemberExpression;
        //        //if (memberExpression == null)
        //        //{
        //        //    throw new NullReferenceException(string.Format("Unable to cast Member Expression for given path: {0}.", sort));
        //        //}
        //        LambdaExpression orderByExp = Expression.Lambda(memberExpression, parameterExpression);
        //        Type rightPropertyType = ((PropertyInfo)(memberExpression.Member)).PropertyType;
        //        MethodCallExpression resultExp = Expression.Call(typeof(Queryable), direction, new Type[] { typ, rightPropertyType }, source.Expression, Expression.Quote(orderByExp));
        //        list = source.Provider.CreateQuery<TEntity>(resultExp);
        //        return new Models.WebGridList<TEntity>()
        //        {
        //            List = (actualIndex <= (totalCount + pageSize) ? list.Skip(actualIndex).Take(pageSize) : list).ToList(),
        //            RowCount = totalCount,
        //            PageSize = pageSize
        //        };
        //    }
        //    catch
        //    {
        //        return new Models.WebGridList<TEntity>()
        //        {
        //            List = source.AsEnumerable().Skip(actualIndex).Take(pageSize).ToList(),
        //            RowCount = totalCount,
        //            PageSize = pageSize
        //        };
        //    }

        //}

        //public static Models.WebGridList<TEntity> PagingAndSorting<TEntity>(this IQueryable<TEntity> source, int pageSize = 10)
        //{
        //    try
        //    {
        //        int totalCount = source.Count();
        //        return new Models.WebGridList<TEntity>()
        //        {
        //            List = source.Take(pageSize).ToList(),
        //            RowCount = totalCount,
        //            PageSize = pageSize
        //        };

        //    }
        //    catch
        //    {
        //        return new Models.WebGridList<TEntity>()
        //        {
        //            List = source.ToList(),
        //            RowCount = 0,
        //            PageSize = pageSize
        //        };
        //    }
        //}

        //private static Expression<Func<TElement, bool>> GetWhereInExpression<TElement, TValue>(Expression<Func<TElement, TValue>> propertySelector, IEnumerable<TValue> values)
        //{
        //    ParameterExpression p = propertySelector.Parameters.Single();
        //    if (!values.Any())
        //        return e => false;

        //    var equals = values.Select(value => (Expression)Expression.Equal(propertySelector.Body, Expression.Constant(value, typeof(TValue))));
        //    var body = equals.Aggregate<Expression>((accumulate, equal) => Expression.Or(accumulate, equal));

        //    return Expression.Lambda<Func<TElement, bool>>(body, p);
        //}

        ///// <summary> 
        ///// Return the element that the specified property's value is contained in the specifiec values 
        ///// </summary> 
        ///// <typeparam name="TElement">The type of the element.</typeparam> 
        ///// <typeparam name="TValue">The type of the values.</typeparam> 
        ///// <param name="source">The source.</param> 
        ///// <param name="propertySelector">The property to be tested.</param> 
        ///// <param name="values">The accepted values of the property.</param> 
        ///// <returns>The accepted elements.</returns> 
        //public static IQueryable<TElement> WhereIn<TElement, TValue>(this IQueryable<TElement> source, Expression<Func<TElement, TValue>> propertySelector, params TValue[] values)
        //{
        //    return source.Where(GetWhereInExpression(propertySelector, values));
        //}

        ///// <summary> 
        ///// Return the element that the specified property's value is contained in the specifiec values 
        ///// </summary> 
        ///// <typeparam name="TElement">The type of the element.</typeparam> 
        ///// <typeparam name="TValue">The type of the values.</typeparam> 
        ///// <param name="source">The source.</param> 
        ///// <param name="propertySelector">The property to be tested.</param> 
        ///// <param name="values">The accepted values of the property.</param> 
        ///// <returns>The accepted elements.</returns> 
        //public static IQueryable<TElement> WhereIn<TElement, TValue>(this IQueryable<TElement> source, Expression<Func<TElement, TValue>> propertySelector, IEnumerable<TValue> values)
        //{
        //    return source.Where(GetWhereInExpression(propertySelector, values));
        //}        
        #endregion

        #region String Extensions
        public static string ConvertToEasternArabicNumerals(this string input)
        {
            string[] indianDigits = new string[] { "٠", "١", "٢", "٣", "٤", "٥", "٦", "٧", "٨", "٩" };
            string[] arabicDigits = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            for (int i = 0; i < 10; i++)
                input = input.Replace(indianDigits[i], arabicDigits[i]);

            return input;
        }
        #endregion


        public static NameValueCollection ToQueryString(this AjaxOptions ajaxOptions)
        {
            NameValueCollection retVal = HttpUtility.ParseQueryString("?");

            foreach (var item in ajaxOptions.ToUnobtrusiveHtmlAttributes())
                retVal.Add(item.Key, item.Value.ToString());


            return retVal;
        }

        public static NameValueCollection ToQueryString(this IDictionary<string, object> dictionary)
        {
            NameValueCollection retVal = HttpUtility.ParseQueryString("?");

            foreach (var item in dictionary)
                retVal.Add(item.Key, item.Value.ToString());

            return retVal;
        }

        public static IDictionary<string, object> DecryptUrlToDictionary(string s)
        {
            IDictionary<string, object> dictionary = new Dictionary<string, object>();
            string clearUrl = StringEncrypter.UrlEncrypter.Decrypt(s);
            HttpUtility.ParseQueryString(clearUrl).CopyTo(dictionary);

            return dictionary;
        }
    }
}