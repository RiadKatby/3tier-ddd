using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;
using System.Xml;
using System.Xml.Linq;

namespace RefactorName.WebApp
{
    public static class WebGridExtensions
    {
        public static HelperResult PagerList(this WebGrid webGrid, WebGridPagerModes mode = WebGridPagerModes.NextPrevious | WebGridPagerModes.Numeric, string firstText = null, string previousText = null, string nextText = null, string lastText = null, int numericLinksCount = 5, dynamic parameters = null, string actionName = null, int? currentPageIndex = null)
        {
            return PagerList(webGrid, mode, firstText, previousText, nextText, lastText, numericLinksCount, true, parameters, actionName, currentPageIndex ?? webGrid.PageIndex);
        }

        private static HelperResult PagerList(WebGrid webGrid, WebGridPagerModes mode, string firstText, string previousText, string nextText, string lastText, int numericLinksCount, bool explicitlyCalled, dynamic parameters, string actionName, int currentPageIndex)
        {

            int currentPage = currentPageIndex;
            int totalPages = webGrid.PageCount;
            int lastPage = totalPages - 1;

            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination pagination-sm");
            var li = new List<TagBuilder>();


            if (ModeEnabled(mode, WebGridPagerModes.FirstLast))
            {
                if (String.IsNullOrEmpty(firstText))
                {
                    firstText = "<<";
                }

                var part = new TagBuilder("li")
                {
                    InnerHtml = GridLink(webGrid, GetListPageUrl(webGrid, 0, parameters, actionName), firstText)
                };

                //if (currentPage == 0)
                //{
                //    part.MergeAttribute("class", "disabled");
                //}

                if (currentPage != 0)
                    li.Add(part);

            }

            if (ModeEnabled(mode, WebGridPagerModes.NextPrevious))
            {
                if (String.IsNullOrEmpty(previousText))
                {
                    previousText = "<";
                }

                int page = currentPage == 0 ? 0 : currentPage - 1;

                var part = new TagBuilder("li")
                {
                    InnerHtml = GridLink(webGrid, GetListPageUrl(webGrid, page, parameters, actionName), previousText)
                };

                //if (currentPage == 0)
                //{
                //    part.MergeAttribute("class", "disabled");
                //}

                if (currentPage != 0)
                    li.Add(part);

            }


            if (ModeEnabled(mode, WebGridPagerModes.Numeric) && (totalPages > 1))
            {
                int last = currentPage + (numericLinksCount / 2);
                int first = last - numericLinksCount + 1;
                if (last > lastPage)
                {
                    first -= last - lastPage;
                    last = lastPage;
                }
                if (first < 0)
                {
                    last = Math.Min(last + (0 - first), lastPage);
                    first = 0;
                }
                for (int i = first; i <= last; i++)
                {

                    var pageText = (i + 1).ToString(CultureInfo.InvariantCulture);
                    var part = new TagBuilder("li")
                    {
                        InnerHtml = GridLink(webGrid, GetListPageUrl(webGrid, i, parameters, actionName), pageText)
                    };

                    if (i == currentPage)
                    {
                        part.MergeAttribute("class", "active");
                    }

                    li.Add(part);

                }
            }

            if (ModeEnabled(mode, WebGridPagerModes.NextPrevious))
            {
                if (String.IsNullOrEmpty(nextText))
                {
                    nextText = ">";
                }

                int page = currentPage == lastPage ? lastPage : currentPage + 1;

                var part = new TagBuilder("li")
                {
                    InnerHtml = GridLink(webGrid, GetListPageUrl(webGrid, page, parameters, actionName), nextText)
                };

                //if (currentPage == lastPage)
                //{
                //    part.MergeAttribute("class", "disabled");
                //}

                if (currentPage != lastPage)
                    li.Add(part);

            }

            if (ModeEnabled(mode, WebGridPagerModes.FirstLast))
            {
                if (String.IsNullOrEmpty(lastText))
                {
                    lastText = ">>";
                }

                var part = new TagBuilder("li")
                {
                    InnerHtml = GridLink(webGrid, GetListPageUrl(webGrid, lastPage, parameters, actionName), lastText)
                };

                //if (currentPage == lastPage)
                //{
                //    part.MergeAttribute("class", "disabled");
                //}

                if (currentPage != lastPage)
                    li.Add(part);

            }

            ul.InnerHtml = string.Join("", li);

            var html = "";
            html = ul.ToString();
            //if (explicitlyCalled && webGrid.IsAjaxEnabled)
            //{
            //    var span = new TagBuilder("span");
            //    span.MergeAttribute("data-swhgajax", "true");
            //    span.MergeAttribute("data-swhgcontainer", webGrid.AjaxUpdateContainerId);
            //    span.MergeAttribute("data-swhgcallback", webGrid.AjaxUpdateCallback);

            //    span.InnerHtml = ul.ToString();
            //    html = span.ToString();

            //}
            //else
            //{
            //    html = ul.ToString();
            //}

            return new HelperResult(writer =>
            {
                writer.Write(html);
            });
        }

        private static String GridLink(WebGrid webGrid, string url, string text)
        {
            TagBuilder builder = new TagBuilder("a");
            builder.SetInnerText(text);
            builder.MergeAttribute("href", url);
            if (webGrid.IsAjaxEnabled)
            {
                builder.MergeAttribute("data-swhglnk", "true");
            }
            return builder.ToString(TagRenderMode.Normal);
        }

        private static bool ModeEnabled(WebGridPagerModes mode, WebGridPagerModes modeCheck)
        {
            return (mode & modeCheck) == modeCheck;
        }

        public static string GetListPageUrl(this WebGrid grid, int pageIndex, dynamic parameters, string actionName)
        {
            string controllerName = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
            if (string.IsNullOrEmpty(actionName)) actionName = "List";

            //int pageNumber = ++pageIndex;

            string originalURL = grid.GetPageUrl(pageIndex);
            RouteValueDictionary parms = ParseParameters(originalURL);

            //parms.Add(grid.PageFieldName, pageNumber);
            //if (!string.IsNullOrEmpty(grid.SortColumn))
            //{
            //    parms.Add(grid.SortFieldName, grid.SortColumn);
            //    var sortDir = grid.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            //    parms.Add(grid.SortDirectionFieldName, sortDir);
            //}

            object obj = parameters;

            if (obj != null)
            {
                var parametersAndValues = from x in obj.GetType().GetProperties()
                                          select new { ParamName = x.Name, ParamValue = x.GetValue(obj, null) };

                foreach (var item in parametersAndValues)
                    //if (item.ParamName.ToLower().Equals("actionname"))
                    //    actionName = item.ParamValue.ToString();
                    //else
                    parms.Add(item.ParamName, item.ParamValue);
            }

            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var url = urlHelper.Action(actionName, controllerName, parms);

            return url;
        }

        public static IHtmlString GetTable(this WebGrid webGrid, string tableStyle = null, string headerStyle = null, string footerStyle = null, string rowStyle = null, string alternatingRowStyle = null, string selectedRowStyle = null, string caption = null, bool displayHeader = true, bool fillEmptyRows = false, string emptyRowCellValue = null, IEnumerable<WebGridColumn> columns = null, IEnumerable<string> exclusions = null, Func<dynamic, object> footer = null, dynamic parameters = null, string actionName = null)
        {
            IHtmlString htmlStr = webGrid.Table(tableStyle, headerStyle, footerStyle, rowStyle, alternatingRowStyle,
                selectedRowStyle, caption, displayHeader, fillEmptyRows, emptyRowCellValue, columns, exclusions, footer);
            int pageNumber = webGrid.PageIndex + 1;

            string controllerName = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
            if (string.IsNullOrEmpty(actionName)) actionName = "List";

            var html = htmlStr.ToHtmlString();
            string strScript = "";
            string strTable = html;

            //extract script tag from the document
            int scriptStartIndex = html.IndexOf("<script");
            if (scriptStartIndex > -1)
            {
                int scriptEndIndex = html.IndexOf("</script>") + 9;
                strScript = html.Substring(scriptStartIndex, scriptEndIndex - scriptStartIndex);
                strTable = html.Substring(scriptEndIndex);
            }

            XDocument xdoc = XDocument.Load(new StringReader(strTable));
            var result = (from x in xdoc.Descendants(XName.Get("a"))
                          where x.Parent.Name == "th"
                          select x)
                          .ToArray();

            foreach (var item in result)
            {
                string href = item.Attribute(XName.Get("href")).Value;
                RouteValueDictionary parms = ParseParameters(href);
                parms.Add(webGrid.PageFieldName, pageNumber);
                parms = PopulateParameters(parms, parameters);

                //var actionNameDictionary = parms.SingleOrDefault(x => x.Key.ToLower().Equals("actionname"));
                //if (!actionNameDictionary.Equals(default(KeyValuePair<string, object>)))
                //{
                //    actionName = actionNameDictionary.Value.ToString();
                //    parms.Remove(actionNameDictionary.Key);
                //}

                UrlHelper urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                var url = urlHelper.Action(actionName, controllerName, parms);

                item.Attribute(XName.Get("href")).Value = url;
            }

            string finalResult = xdoc.ToString();
            return new MvcHtmlString(strScript + finalResult);
        }

        private static RouteValueDictionary ParseParameters(string url)
        {
            RouteValueDictionary retVal = new RouteValueDictionary();

            string[] urlParts = url.Split('?');
            NameValueCollection q = HttpUtility.ParseQueryString(urlParts[1]);

            foreach (var item in q.AllKeys)
                retVal.Add(item, q[item]);

            return retVal;
        }

        private static RouteValueDictionary PopulateParameters(RouteValueDictionary routeData, dynamic parameters)
        {
            object obj = parameters;

            if (obj != null)
            {
                var parametersAndValues = from x in obj.GetType().GetProperties()
                                          select new { ParamName = x.Name, ParamValue = Convert.ToString(x.GetValue(obj, null)) };

                foreach (var pv in parametersAndValues)
                    if (routeData.ContainsKey(pv.ParamName))
                        routeData[pv.ParamName] = pv.ParamValue;
                    else
                        routeData.Add(pv.ParamName, pv.ParamValue);
            }

            return routeData;
        }

        public static IHtmlString GetTableWithPager(this WebGrid webGrid,
            string tableStyle = null, string headerStyle = null, string footerStyle = null, string rowStyle = null,
            string alternatingRowStyle = null, string selectedRowStyle = null, string caption = null,
            bool displayHeader = true, bool fillEmptyRows = false, string emptyRowCellValue = null,
            IEnumerable<WebGridColumn> columns = null, IEnumerable<string> exclusions = null,
            Func<dynamic, object> footer = null,
            WebGridPagerModes mode = WebGridPagerModes.NextPrevious | WebGridPagerModes.Numeric,
            string firstText = null,
            string previousText = null,
            string nextText = null,
            string lastText = null,
            int numericLinksCount = 5,
            object parameters = null,
            string actionName = null,
            int? currerntPageIndex = null)
        {
            //get table with pager inside the footer
            IHtmlString strTable = GetTable(webGrid, tableStyle, headerStyle, footerStyle, rowStyle, alternatingRowStyle,
                selectedRowStyle, caption, displayHeader, fillEmptyRows, emptyRowCellValue, columns, exclusions
                , x => PagerList(webGrid, mode, firstText, previousText, nextText, lastText, numericLinksCount, parameters, actionName, currerntPageIndex ?? webGrid.PageIndex),
                parameters, actionName);
            //string 
            return strTable;
        }
    }
}