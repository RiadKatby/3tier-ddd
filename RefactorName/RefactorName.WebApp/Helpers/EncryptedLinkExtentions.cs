using RefactorName.WebApp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Mvc.Html;

namespace RefactorName.WebApp
{
    /// <summary>
    /// Represents support for encrypted HTML links in an application.
    /// </summary>
    public static class EncryptedLinkExtentions
    {
        private static readonly IEncryptString encrypter = new ConfigurationBasedStringEncrypter();

        /// <summary>
        /// Returns an anchor element (a element) that contains the virtual path of the specified action, with an encrypted routeValues.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="linkText">The inner text of the anchor element.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="routeValues">An object that contains the parameters for a route. The parameters are retrieved through reflection by examining the properties of the object. The object is typically created by using object initializer syntax.</param>
        /// <returns>An anchor element (a element) with encrypted routeValues.</returns>
        /// <exception cref="System.ArgumentException">The linkText parameter is null or empty.</exception>
        public static MvcHtmlString EncryptedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues)
        {
            RouteValueDictionary parameters = HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues);
            return EncryptedActionLink(htmlHelper, linkText, actionName, parameters);
        }

        /// <summary>
        /// Returns an anchor element (a element) that contains the virtual path of the specified action, with an encrypted routeValues.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="linkText">The inner text of the anchor element.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="routeValues">An object that contains the parameters for a route.</param>
        /// <returns>An anchor element (a element) with encrypted routeValues.</returns>
        /// <exception cref="System.ArgumentException">The linkText parameter is null or empty.</exception>
        public static MvcHtmlString EncryptedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues)
        {
            RouteValueDictionary parameters = encrypter.EncryptRouteValueDictionary(routeValues);
            return htmlHelper.ActionLink(linkText, actionName, parameters);
        }

        /// <summary>
        /// Returns an anchor element (a element) that contains the virtual path of the specified action, with encrypted routeValues.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="linkText">The inner text of the anchor element.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="routeValues">An object that contains the parameters for a route. The parameters are retrieved through reflection by examining the properties of the object. The object is typically created by using object initializer syntax.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes for the element. The attributes are retrieved through reflection by examining the properties of the object. The object is typically created by using object initializer syntax.</param>
        /// <returns>An anchor element (a element) with encrypted routeValues.</returns>
        /// <exception cref="System.ArgumentException">The linkText parameter is null or empty.</exception>
        public static MvcHtmlString EncryptedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues, object htmlAttributes)
        {
            RouteValueDictionary parameters = HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues);
            RouteValueDictionary attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            return EncryptedActionLink(htmlHelper, linkText, actionName, parameters, attributes);
        }

        /// <summary>
        /// Returns an anchor element (a element) that contains the virtual path of the specified action, with encrypted routeValues.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="linkText">The inner text of the anchor element.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="routeValues">An object that contains the parameters for a route.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns>An anchor element (a element) with encrypted routeValues.</returns>
        /// <exception cref="System.ArgumentException">The linkText parameter is null or empty.</exception>
        public static MvcHtmlString EncryptedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            RouteValueDictionary parameters = encrypter.EncryptRouteValueDictionary(routeValues);
            return htmlHelper.ActionLink(linkText, actionName, parameters, htmlAttributes);
        }

        /// <summary>
        /// Returns an anchor element (a element) that contains the virtual path of the specified action, with encrypted routeValues.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="linkText">The inner text of the anchor element.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">An object that contains the parameters for a route. The parameters are retrieved through reflection by examining the properties of the object. The object is typically created by using object initializer syntax.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns>An anchor element (a element) with encrypted routeValues.</returns>
        /// <exception cref="System.ArgumentException">The linkText parameter is null or empty.</exception>
        public static MvcHtmlString EncryptedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            RouteValueDictionary parameters = HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues);
            RouteValueDictionary attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            return EncryptedActionLink(htmlHelper, linkText, actionName, controllerName, parameters, attributes);
        }

        /// <summary>
        /// Returns an anchor element (a element) that contains the virtual path of the specified action with encrypted routeValues.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="linkText">The inner text of the anchor element.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">An object that contains the parameters for a route.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns>An anchor element (a element) with encrypted routeValues..</returns>
        /// <exception cref="System.ArgumentException">The linkText parameter is null or empty.</exception>
        public static MvcHtmlString EncryptedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            RouteValueDictionary parameters = encrypter.EncryptRouteValueDictionary(routeValues);
            return htmlHelper.ActionLink(linkText, actionName, controllerName, parameters, htmlAttributes);
        }

        /// <summary>
        /// Returns an anchor element (a element) that contains the virtual path of the specified action, with encrypted routeValues.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="linkText">The inner text of the anchor element.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="protocol">The protocol for the URL, such as "http" or "https".</param>
        /// <param name="hostName">The host name for the URL.</param>
        /// <param name="fragment">The URL fragment name (the anchor name).</param>
        /// <param name="routeValues">An object that contains the parameters for a route. The parameters are retrieved through reflection by examining the properties of the object. The object is typically created by using object initializer syntax.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns>An anchor element (a element) with encrypted routeValues.</returns>
        /// <exception cref="System.ArgumentException">The linkText parameter is null or empty.</exception>
        public static MvcHtmlString EncryptedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes)
        {
            RouteValueDictionary parameters = HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues);
            RouteValueDictionary attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            return EncryptedActionLink(htmlHelper, linkText, actionName, controllerName, protocol, hostName, fragment, parameters, attributes);
        }

        /// <summary>
        /// Returns an anchor element (a element) that contains the virtual path of the specified action, with encrypted routeValues.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="linkText">The inner text of the anchor element.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="protocol">The protocol for the URL, such as "http" or "https".</param>
        /// <param name="hostName">The host name for the URL.</param>
        /// <param name="fragment">The URL fragment name (the anchor name).</param>
        /// <param name="routeValues">An object that contains the parameters for a route.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns>An anchor element (a element) with encrypted routeValues.</returns>
        /// <exception cref="System.ArgumentException">The linkText parameter is null or empty.</exception>
        public static MvcHtmlString EncryptedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            RouteValueDictionary parameters = encrypter.EncryptRouteValueDictionary(routeValues);
            return htmlHelper.ActionLink(linkText, actionName, controllerName, protocol, hostName, fragment, parameters, htmlAttributes);
        }
    }
}