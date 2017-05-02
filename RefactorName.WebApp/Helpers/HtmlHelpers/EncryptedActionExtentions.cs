using RefactorName.WebApp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RefactorName.WebApp.Helpers
{
    /// <summary>
    /// Defines extention method for <see cref="UrlHelper"/> to generate encrypted Urls.
    /// </summary>
    public static class EncryptedActionExtentions
    {
        private static readonly IEncryptString encrypter = new ConfigurationBasedStringEncrypter();

        /// <summary>
        /// Generates a fully qualified and encrypted URL to an action method by using the specified action name and route values.
        /// </summary>
        /// <param name="actionName">The name of the action method.</param>
        /// <param name="routeValues">An object that contains the parameters for a route. The parameters are retrieved through reflection by examining the properties of the object. The object is typically created by using object initializer syntax.</param>
        /// <returns>The fully qualified and encrypted URL to an action method.</returns>
        public static string EncryptedAction(this UrlHelper helper, string actionName, object routeValues)
        {
            RouteValueDictionary parameters = HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues);
            return EncryptedAction(helper, actionName, parameters);
        }

        /// <summary>
        /// Generates a fully qualified and encrypted URL to an action method for the specified action name and route values.
        /// </summary>
        /// <param name="actionName">The name of the action method.</param>
        /// <param name="routeValues">An object that contains the parameters for a route.</param>
        /// <returns>The fully qualified and encrypted URL to an action method.</returns>
        public static string EncryptedAction(this UrlHelper helper, string actionName, RouteValueDictionary routeValues)
        {
            RouteValueDictionary parameters = encrypter.EncryptRouteValueDictionary(routeValues);
            return helper.Action(actionName, parameters);
        }

        /// <summary>
        /// Generates a fully qualified and encrypted URL to an action method by using the specified action name, controller name, and route values.
        /// </summary>
        /// <param name="actionName">The name of the action method.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">An object that contains the parameters for a route which will be encrypted.</param>
        /// <returns>The fully qualified and encrypted URL to an action method.</returns>
        public static string EncryptedAction(this UrlHelper helper, string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            RouteValueDictionary parameters = encrypter.EncryptRouteValueDictionary(routeValues);
            return helper.Action(actionName, controllerName, parameters);
        }

        /// <summary>
        /// Generates a fully qualified and encrypted URL to an action method by using the specified action name, controller name, and route values.
        /// </summary>
        /// <param name="actionName">The name of the action method.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">An object that contains the parameters for a route which will be encrypted. The parameters are retrieved through reflection by examining the properties of the object. The object is typically created by using object initializer syntax.</param>
        /// <returns>The fully qualified and encrypted URL to an action method.</returns>
        public static string EncryptedAction(this UrlHelper helper, string actionName, string controllerName, object routeValues)
        {
            RouteValueDictionary parameters = HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues);
            return EncryptedAction(helper, actionName, controllerName, parameters);
        }

        /// <summary>
        /// Generates a fully qualified and encrypted URL to an action method by using the specified action name, controller name, route values, and protocol to use.
        /// </summary>
        /// <param name="actionName">The name of the action method.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">An object that contains the parameters for a route which will be encrypted. The parameters are retrieved through reflection by examining the properties of the object. The object is typically created by using object initializer syntax.</param>
        /// <param name="protocol">The protocol for the URL, such as "http" or "https".</param>
        /// <returns>The fully qualified and encrypted URL to an action method.</returns>
        public static string EncryptedAction(this UrlHelper helper, string actionName, string controllerName, object routeValues, string protocol)
        {
            RouteValueDictionary routeValuesDic = HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues);
            RouteValueDictionary parameters = encrypter.EncryptRouteValueDictionary(routeValuesDic);
            return helper.Action(actionName, controllerName, parameters, protocol);
        }

        /// <summary>
        /// Generates a fully qualified and encrypted URL for an action method by using the specified action name, controller name, route values, protocol to use, and host name.
        /// </summary>
        /// <param name="actionName">The name of the action method.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">An object that contains the parameters for a route which will be encrypted.</param>
        /// <param name="protocol">The protocol for the URL, such as "http" or "https".</param>
        /// <param name="hostName">The host name for the URL.</param>
        /// <returns>The fully qualified and encrypted URL to an action method.</returns>
        public static string EncryptedAction(this UrlHelper helper, string actionName, string controllerName, RouteValueDictionary routeValues, string protocol, string hostName)
        {
            RouteValueDictionary parameters = encrypter.EncryptRouteValueDictionary(routeValues);
            return helper.Action(actionName, controllerName, parameters, protocol, hostName);
        }
    }
}