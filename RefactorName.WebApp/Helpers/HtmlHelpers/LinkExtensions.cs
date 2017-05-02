using RefactorName.WebApp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace MvcHtmlHelpers
{
    public static class LinkExtensions
    {
        /// <summary>
        /// Returns anchor element with bootstrap modal confirmation befor navigation and with encrypted route values
        /// </summary>
        /// <param name="linkInnerHtml">Anchor inner html</param>
        /// <param name="confirmationTitle">bootstrap modal title</param>
        /// <param name="confirmationMessage">bootstrap modal confirmation message</param>
        /// <param name="actionName">Url Action name</param>
        /// <param name="controllerName">Url Controller name</param>
        /// <param name="routeValues">Url rout values</param>
        /// <param name="htmlAttributes">custom httmlAttributes applied to the anchor element</param>
        /// <param name="onlyForRoles">comma delimited roles to display the button for</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString EncryptedAcionLinkWithConfirm(this HtmlHelper helper, string linkInnerHtml,
            string confirmationTitle, string confirmationMessage,
            string actionName, string controllerName = null, object routeValues = null, object htmlAttributes = null, string onlyForRoles = null)
        {
            return AcionLinkWithConfirm(helper, linkInnerHtml, confirmationTitle, confirmationMessage, actionName, controllerName, Util.EncryptRouteValues(routeValues), htmlAttributes, onlyForRoles);
        }

        /// <summary>
        /// Returns ajax anchor element with bootstrap modal confirmation befor request and with encrypted route values
        /// </summary>
        /// <param name="linkInnerHtml">Anchor inner html</param>
        /// <param name="confirmationTitle">bootstrap modal title</param>
        /// <param name="confirmationMessage">bootstrap modal confirmation message</param>
        /// <param name="actionName">Url Action name</param>
        /// <param name="controllerName">Url Controller name</param>
        /// <param name="routeValues">Url rout values</param>
        /// <param name="htmlAttributes">custom httmlAttributes applied to the anchor element</param>
        /// <param name="onlyForRoles">comma delimited roles to display the button for</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString EncryptedAjaxLinkWithConfirm(this AjaxHelper helper, string linkInnerHtml,
            string confirmationTitle, string confirmationMessage,
            string actionName, string controllerName = null, object routeValues = null, AjaxOptions ajaxOptions = null, object htmlAttributes = null, string onlyForRoles = null)
        {
            return AjaxLinkWithConfirm(helper, linkInnerHtml, confirmationTitle, confirmationMessage, actionName, controllerName, Util.EncryptRouteValues(routeValues), ajaxOptions, htmlAttributes, onlyForRoles);
        }

        /// <summary>
        /// Returns Exact same as ActionLink but takes inner html instead of inner text and with encrypted route values
        /// </summary>
        /// <param name="linkInnerHtml">Anchor inner html</param>
        /// <param name="actionName">Url Action name</param>
        /// <param name="controllerName">Url Controller name</param>
        /// <param name="routeValues">Url rout values</param>
        /// <param name="htmlAttributes">custom httmlAttributes applied to the anchor element</param>
        /// <param name="onlyForRoles">comma delimited roles to display the button for</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString EncryptedAcionLink(this HtmlHelper htmlHelper, string linkInnerHtml,
            string actionName, string controllerName = null, object routeValues = null, object htmlAttributes = null, string onlyForRoles = null)
        {
            return GenerateAcionLink(htmlHelper.RouteCollection, htmlHelper.ViewContext.RequestContext, linkInnerHtml, actionName, controllerName, null, null, null, HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), onlyForRoles, true /* encrypted */);
        }

        /// <summary>
        /// Returns Exact same as AjaxActionLink but takes inner html instead of inner text and with encrypted route values
        /// </summary>
        /// <param name="linkInnerHtml">Anchor inner html</param>
        /// <param name="actionName">Url Action name</param>
        /// <param name="controllerName">Url Controller name</param>
        /// <param name="routeValues">Url rout values</param>
        /// <param name="ajaxOptions">AjaxOptions</param>
        /// <param name="htmlAttributes">custom httmlAttributes applied to the anchor element</param>
        /// <param name="onlyForRoles">comma delimited roles to display the button for</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString EncryptedAjaxLink(this AjaxHelper helper, string linkInnerHtml,
            string actionName, string controllerName = null, object routeValues = null, AjaxOptions ajaxOptions = null, object htmlAttributes = null, string onlyForRoles = null)
        {
            return AjaxLinkWithInnerHtml(helper, linkInnerHtml, actionName, controllerName, Util.EncryptRouteValues(routeValues), ajaxOptions, htmlAttributes, onlyForRoles);
        }

        public static MvcHtmlString EncryptedAcionLink(this HtmlHelper htmlHelper, string linkInnerHtml, string actionName, string controllerName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes, string onlyForRoles)
        {
            return GenerateAcionLink(htmlHelper.RouteCollection, htmlHelper.ViewContext.RequestContext, linkInnerHtml, actionName, controllerName, protocol, hostName, fragment, HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), onlyForRoles, true /* encrypted */);
        }

        public static MvcHtmlString EncryptedAcionLink(this HtmlHelper htmlHelper, string linkInnerHtml, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes, string onlyForRoles)
        {
            return GenerateAcionLink(htmlHelper.RouteCollection, htmlHelper.ViewContext.RequestContext, linkInnerHtml, actionName, controllerName, protocol, hostName, fragment, HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), onlyForRoles, true /* encrypted */);
        }

        /// <summary>
        /// Returns anchor element with bootstrap modal confirmation befor navigation
        /// </summary>
        /// <param name="linkInnerHtml">Anchor inner html</param>
        /// <param name="confirmationTitle">bootstrap modal title</param>
        /// <param name="confirmationMessage">bootstrap modal confirmation message</param>
        /// <param name="actionName">Url Action name</param>
        /// <param name="controllerName">Url Controller name</param>
        /// <param name="routeValues">Url rout values</param>
        /// <param name="htmlAttributes">custom httmlAttributes applied to the anchor element</param>
        /// <param name="onlyForRoles">comma delimited roles to display the button for</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString AcionLinkWithConfirm(this HtmlHelper helper, string linkInnerHtml,
            string confirmationTitle, string confirmationMessage,
            string actionName, string controllerName = null, object routeValues = null, object htmlAttributes = null, string onlyForRoles = null)
        {
            RouteValueDictionary customAttributes = AddConfirmationAttributes(htmlAttributes, confirmationTitle, confirmationMessage);
            RouteValueDictionary d;
            if (routeValues is RouteValueDictionary)
                d = routeValues as RouteValueDictionary;
            else
                d = new RouteValueDictionary(routeValues);
            return GenerateAcionLink(helper.RouteCollection, helper.ViewContext.RequestContext, linkInnerHtml, actionName, controllerName, null, null, null, d, customAttributes, onlyForRoles, false /* encrypted */);
        }

        /// <summary>
        /// Returns ajax anchor element with bootstrap modal confirmation befor request
        /// </summary>
        /// <param name="linkInnerHtml">Anchor inner html</param>
        /// <param name="confirmationTitle">bootstrap modal title</param>
        /// <param name="confirmationMessage">bootstrap modal confirmation message</param>
        /// <param name="actionName">Url Action name</param>
        /// <param name="controllerName">Url Controller name</param>
        /// <param name="routeValues">Url rout values</param>
        /// <param name="htmlAttributes">custom httmlAttributes applied to the anchor element</param>
        /// <param name="onlyForRoles">comma delimited roles to display the button for</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString AjaxLinkWithConfirm(this AjaxHelper helper, string linkInnerHtml,
            string confirmationTitle, string confirmationMessage,
            string actionName, string controllerName = null, object routeValues = null, AjaxOptions ajaxOptions = null, object htmlAttributes = null, string onlyForRoles = null)
        {
            RouteValueDictionary customAttributes = AddConfirmationAttributes(htmlAttributes, confirmationTitle, confirmationMessage);
            ajaxOptions = (ajaxOptions ?? new AjaxOptions());
            if (string.IsNullOrWhiteSpace(ajaxOptions.OnBegin))
                ajaxOptions.OnBegin = "ReturnFalse";

            return AjaxLinkWithInnerHtml(helper, linkInnerHtml, actionName, controllerName, routeValues, ajaxOptions, customAttributes, onlyForRoles);
        }

        /// <summary>
        /// Returns Exact same as ActionLink but takes inner html instead of inner text
        /// </summary>
        /// <param name="linkInnerHtml">Anchor inner html</param>
        /// <param name="actionName">Url Action name</param>
        /// <param name="controllerName">Url Controller name</param>
        /// <param name="routeValues">Url rout values</param>
        /// <param name="htmlAttributes">custom httmlAttributes applied to the anchor element</param>
        /// <param name="onlyForRoles">comma delimited roles to display the button for</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString AcionLinkWithInnerHtml(this HtmlHelper htmlHelper, string linkInnerHtml,
            string actionName, string controllerName = null, object routeValues = null, object htmlAttributes = null, string onlyForRoles = null)
        {
            return GenerateAcionLink(htmlHelper.RouteCollection, htmlHelper.ViewContext.RequestContext, linkInnerHtml, actionName, controllerName, null, null, null, HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), onlyForRoles, false /* encrypted */);
        }

        /// <summary>
        /// Returns Exact same as AjaxActionLink but takes inner html instead of inner text
        /// </summary>
        /// <param name="linkInnerHtml">Anchor inner html</param>
        /// <param name="actionName">Url Action name</param>
        /// <param name="controllerName">Url Controller name</param>
        /// <param name="routeValues">Url rout values</param>
        /// <param name="ajaxOptions">AjaxOptions</param>
        /// <param name="htmlAttributes">custom httmlAttributes applied to the anchor element</param>
        /// <param name="onlyForRoles">comma delimited roles to display the button for</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString AjaxLinkWithInnerHtml(this AjaxHelper helper, string linkInnerHtml,
            string actionName, string controllerName = null, object routeValues = null, AjaxOptions ajaxOptions = null, object htmlAttributes = null, string onlyForRoles = null)
        {
            RouteValueDictionary customAttributes;
            if (htmlAttributes is RouteValueDictionary)
                customAttributes = htmlAttributes as RouteValueDictionary;
            else
                customAttributes = new RouteValueDictionary(htmlAttributes);

            ajaxOptions = (ajaxOptions ?? new AjaxOptions());

            var ajaxOptionAttributes = ajaxOptions.ToUnobtrusiveHtmlAttributes();
            //add ajax attributes to htmlAttributes
            foreach (var item in ajaxOptionAttributes)
                customAttributes[item.Key] = item.Value;

            RouteValueDictionary realRoutValues;
            if (routeValues is RouteValueDictionary)
                realRoutValues = routeValues as RouteValueDictionary;
            else
                realRoutValues = new RouteValueDictionary(routeValues);

            return GenerateAcionLink(helper.RouteCollection, helper.ViewContext.RequestContext, linkInnerHtml, actionName, controllerName, null, null, null, realRoutValues, customAttributes, onlyForRoles, false /* encrypted */);
        }

        public static MvcHtmlString AcionLinkWithInnerHtml(this HtmlHelper htmlHelper, string linkInnerHtml, string actionName, string controllerName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes, string onlyForRoles)
        {
            return GenerateAcionLink(htmlHelper.RouteCollection, htmlHelper.ViewContext.RequestContext, linkInnerHtml, actionName, controllerName, protocol, hostName, fragment, HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), onlyForRoles, false /* encrypted */);
        }

        public static MvcHtmlString AcionLinkWithInnerHtml(this HtmlHelper htmlHelper, string linkInnerHtml, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes, string onlyForRoles)
        {
            return GenerateAcionLink(htmlHelper.RouteCollection, htmlHelper.ViewContext.RequestContext, linkInnerHtml, actionName, controllerName, protocol, hostName, fragment, HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), onlyForRoles, false /* encrypted */);
        }

        private static MvcHtmlString GenerateAcionLink(RouteCollection routeCollection, RequestContext requestContext, string linkInnerHtml, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes, string onlyForRoles, bool encrypted)
        {
            //check roles
            if (!string.IsNullOrWhiteSpace(onlyForRoles) &&
                (
                !HttpContext.Current.Request.IsAuthenticated ||
                (HttpContext.Current.User as UserProfilePrincipal) != null && !(HttpContext.Current.User as UserProfilePrincipal).IsInRoles(onlyForRoles))
                )
                return new MvcHtmlString("");

            if (encrypted)
                routeValues = Util.EncryptRouteValues(routeValues);

            string url = UrlHelper.GenerateUrl(null, actionName, controllerName, protocol, hostName, fragment, routeValues, routeCollection, requestContext, false);
            TagBuilder tagBuilder = new TagBuilder("a")
            {
                InnerHtml = (!String.IsNullOrEmpty(linkInnerHtml)) ? linkInnerHtml : String.Empty
            };
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("href", url);
            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }

        private static RouteValueDictionary AddConfirmationAttributes(object htmlAttributes, string confirmationTitle, string confirmationMessage)
        {
            RouteValueDictionary result;
            if (htmlAttributes is RouteValueDictionary)
                result = htmlAttributes as RouteValueDictionary;
            else
                result = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            result["class"] += " mci-confirm";
            result["title"] = confirmationTitle;
            result["rel"] = confirmationMessage;

            return result;
        }

    }
}