using RefactorName.WebApp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Mvc.Html;
using System.Web.Mvc.Ajax;

namespace RefactorName.WebApp.Helpers
{
    /// <summary>
    /// Represents support for encrypted HTML and AJAX links in an application.
    /// </summary>
    public static class LinkExtensions
    {
        /// <summary>
        /// Determines if current user is authenticated and has the specified permission codes.
        /// </summary>
        /// <param name="permissionCodes">permission codes to be challenged against current user, no permission return true if current user is authenticated.</param>
        /// <returns>True, if current user authenticated and either has the specified permission, or nor permission code specified. False otherwise.</returns>
        private static bool ChallengePermissionCode(string[] permissionCodes)
        {
            var currentUser = HttpContext.Current.User;

            if (permissionCodes == null)
                return true;

            if (permissionCodes.Length == 0)
                return true;

            if (!currentUser.Identity.IsAuthenticated)
                return false;

            if (permissionCodes.Any(permissionCode => currentUser.IsInRole(permissionCode)))
                return true;

            return false;
        }

        #region Encrypted Action Link
        /// <summary>
        /// Returns an encrypted HREF anchor element (a element) for the specified inner HTML, action, controller, route values as a route value dictionary, and HTML attributes as a dictionary.
        /// </summary>
        /// <param name="helper">The HTML helper instance that this method extends.</param>
        /// <param name="innerHtml">The inner HTML tag of the anchor element.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="permissionCodes">permissions that have to be checked to show the element.</param>
        /// <returns>An encrypted HREF anchor element (a element) if current user is authenticated and has the specified permission codes.</returns>
        public static MvcHtmlString Link(this HtmlHelper helper, string innerHtml, string actionName, string controllerName, params string[] permissionCodes)
        {
            return Link(helper, innerHtml, actionName, controllerName, null, null, null, null, null, permissionCodes);
        }

        /// <summary>
        /// Returns an encrypted HREF anchor element (a element) for the specified inner HTML, action, controller, route values as a route value dictionary, and HTML attributes as a dictionary.
        /// </summary>
        /// <param name="helper">The HTML helper instance that this method extends.</param>
        /// <param name="innerHtml">The inner HTML tag of the anchor element.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="routeValues">An object that contains the parameters for a route.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <param name="permissionCodes">permissions that have to be checked to show the element.</param>
        /// <returns>An encrypted HREF anchor element (a element) if current user is authenticated and has the specified permission codes.</returns>
        public static MvcHtmlString Link(this HtmlHelper helper, string innerHtml, string actionName, object routeValues, object htmlAttributes, params string[] permissionCodes)
        {
            var parameters = HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues);
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            return Link(helper, innerHtml, actionName, null, parameters, attributes, permissionCodes);
        }

        /// <summary>
        /// Returns an encrypted HREF anchor element (a element) for the specified inner HTML, action, controller, route values as a route value dictionary, and HTML attributes as a dictionary.
        /// </summary>
        /// <param name="helper">The HTML helper instance that this method extends.</param>
        /// <param name="innerHtml">The inner HTML tag of the anchor element.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">An object that contains the parameters for a route.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <param name="permissionCodes">permissions that have to be checked to show the element.</param>
        /// <returns>An encrypted HREF anchor element (a element) if current user is authenticated and has the specified permission codes.</returns>
        public static MvcHtmlString Link(this HtmlHelper helper, string innerHtml, string actionName, string controllerName, object routeValues, object htmlAttributes, params string[] permissionCodes)
        {
            RouteValueDictionary parameters = HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues);
            RouteValueDictionary attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            return Link(helper, innerHtml, actionName, controllerName, parameters, attributes, permissionCodes);
        }

        /// <summary>
        /// Returns an encrypted HREF anchor element (a element) for the specified inner HTML, action, controller, route values as a route value dictionary, and HTML attributes as a dictionary.
        /// </summary>
        /// <param name="helper">The HTML helper instance that this method extends.</param>
        /// <param name="innerHtml">The inner HTML tag of the anchor element.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">An object that contains the parameters for a route.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <param name="permissionCodes">permissions that have to be checked to show the element.</param>
        /// <returns>An encrypted HREF anchor element (a element) if current user is authenticated and has the specified permission codes.</returns>
        public static MvcHtmlString Link(this HtmlHelper helper, string innerHtml, string actionName, string controllerName, RouteValueDictionary routeValues, RouteValueDictionary htmlAttributes, params string[] permissionCodes)
        {
            return GenerateEncryptedLinkInternal(helper.ViewContext.RequestContext, helper.RouteCollection, innerHtml, null, actionName, controllerName, null, null, null, routeValues, htmlAttributes, permissionCodes);
        }

        /// <summary>
        /// Returns an encrypted HREF anchor element (a element) for the specified inner HTML, action, controller, route values as a route value dictionary, and HTML attributes as a dictionary.
        /// </summary>
        /// <param name="helper">The HTML helper instance that this method extends.</param>
        /// <param name="innerHtml">The inner HTML tag of the anchor element.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">An object that contains the parameters for a route.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <param name="permissionCodes">permissions that have to be checked to show the element.</param>
        /// <param name="protocol">The protocol for the URL, such as "http" or "https".</param>
        /// <param name="hostName">The host name for the URL.</param>
        /// <param name="fragment">The URL fragment name (the anchor name).</param>
        /// <returns>An encrypted HREF anchor element (a element) if current user is authenticated and has the specified permission codes.</returns>
        public static MvcHtmlString Link(this HtmlHelper helper, string innerHtml, string actionName, string controllerName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes, params string[] permissionCodes)
        {
            RouteValueDictionary parameters = HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues);
            RouteValueDictionary attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            return Link(helper, innerHtml, actionName, controllerName, protocol, hostName, fragment, parameters, attributes, permissionCodes);
        }

        /// <summary>
        /// Returns an encrypted HREF anchor element (a element) for the specified inner HTML, action, controller, route values as a route value dictionary, and HTML attributes as a dictionary.
        /// </summary>
        /// <param name="helper">The HTML helper instance that this method extends.</param>
        /// <param name="innerHtml">The inner HTML tag of the anchor element.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">An object that contains the parameters for a route.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <param name="permissionCodes">permissions that have to be checked to show the element.</param>
        /// <param name="protocol">The protocol for the URL, such as "http" or "https".</param>
        /// <param name="hostName">The host name for the URL.</param>
        /// <param name="fragment">The URL fragment name (the anchor name).</param>
        /// <returns>An encrypted HREF anchor element (a element) if current user is authenticated and has the specified permission codes.</returns>
        public static MvcHtmlString Link(this HtmlHelper helper, string innerHtml, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes, params string[] permissionCodes)
        {
            return GenerateLinkInternal(helper.ViewContext.RequestContext, helper.RouteCollection, innerHtml, null, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes, permissionCodes);
        }

        #endregion Encrypted Action Link

        #region Encrypted Ajax Action Link

        /// <summary>
        /// Returns an anchor element that contains the an encrypted URL to the specified action method; when the action link is clicked, the action method is invoked asynchronously by using JavaScript.
        /// </summary>
        /// <param name="helper">The AJAX helper.</param>
        /// <param name="innerHtml">The inner HTML of the anchor element.</param>
        /// <param name="actionName"> The name of the action method.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">An object that contains the parameters for a route. The parameters will be encrypted into one (q) parameter. This object is typically created by using object initializer syntax.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <param name="ajaxOptions">An object that provides options for the asynchronous request.</param>
        /// <param name="permissionCodes">permissions that have to be checked to show the element.</param>
        /// <returns>An encrypted HREF anchor element (a element) if current user is authenticated and has the specified permission codes.</returns>
        public static MvcHtmlString Link(this AjaxHelper helper, string innerHtml, string actionName, string controllerName, object routeValues, object htmlAttributes, AjaxOptions ajaxOptions, params string[] permissionCodes)
        {
            RouteValueDictionary parameters = HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues);
            RouteValueDictionary attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            permissionCodes = permissionCodes ?? new string[] { };

            return Link(helper, innerHtml, actionName, controllerName, parameters, attributes, ajaxOptions, permissionCodes);
        }

        /// <summary>
        /// Returns an anchor element that contains the an encrypted URL to the specified action method; when the action link is clicked, the action method is invoked asynchronously by using JavaScript.
        /// </summary>
        /// <param name="helper">The AJAX helper.</param>
        /// <param name="innerHtml">The inner HTML of the anchor element.</param>
        /// <param name="actionName"> The name of the action method.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">An object that contains the parameters for a route. The parameters will be encrypted into one (q) parameter.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <param name="ajaxOptions">An object that provides options for the asynchronous request.</param>
        /// <param name="permissionCodes">permissions that have to be checked to show the element.</param>
        /// <returns>An encrypted HREF anchor element (a element) if current user is authenticated and has the specified permission codes.</returns>
        public static MvcHtmlString Link(this AjaxHelper helper, string innerHtml, string actionName, string controllerName, RouteValueDictionary routeValues, RouteValueDictionary htmlAttributes, AjaxOptions ajaxOptions, params string[] permissionCodes)
        {
            ajaxOptions = (ajaxOptions ?? new AjaxOptions());

            var ajaxOptionAttributes = ajaxOptions.ToUnobtrusiveHtmlAttributes();
            //add ajax attributes to htmlAttributes
            foreach (var item in ajaxOptionAttributes)
                htmlAttributes[item.Key] = item.Value;

            return GenerateEncryptedLinkInternal(helper.ViewContext.RequestContext, helper.RouteCollection, innerHtml, null, actionName, controllerName, null, null, null, routeValues, htmlAttributes, permissionCodes);
        }

        /// <summary>
        /// Returns an anchor element that contains the an encrypted URL to the specified action method; when the action link is clicked, the action method is invoked asynchronously by using JavaScript.
        /// </summary>
        /// <param name="helper">The AJAX helper.</param>
        /// <param name="innerHtml">The inner HTML of the anchor element.</param>
        /// <param name="actionName"> The name of the action method.</param>
        /// <param name="routeValues">An object that contains the parameters for a route. The parameters will be encrypted into one (q) parameter.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <param name="ajaxOptions">An object that provides options for the asynchronous request.</param>
        /// <param name="permissionCodes">permissions that have to be checked to show the element.</param>
        /// <returns>An encrypted HREF anchor element (a element) if current user is authenticated and has the specified permission codes.</returns>
        public static MvcHtmlString Link(this AjaxHelper helper, string innerHtml, string actionName, object routeValues, object htmlAttributes, AjaxOptions ajaxOptions, params string[] permissionCodes)
        {
            return helper.Link(innerHtml, actionName, null, routeValues, htmlAttributes, ajaxOptions, permissionCodes);
        }
        #endregion

        #region HTML Action Link

        /// <summary>
        /// Returns an anchor element (a element) for the specified inner HTML, action, controller, route values, and HTML attributes.
        /// </summary>
        /// <param name="helper">The HTML helper instance that this method extends.</param>
        /// <param name="innerHtml">The inner HTML element of the anchor element.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="routeValues">An object that contains the parameters for a route. The parameters are retrieved through reflection by examining the properties of the object. The object is typically created by using object initializer syntax.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <param name="permissionCodes">permissions that have to be checked to show the element.</param>
        /// <returns>An anchor element (a element) if current user is authenticated and has the specified permission codes.</returns>
        public static MvcHtmlString HtmlLink(this HtmlHelper helper, string innerHtml, string actionName, object htmlAttributes, params string[] permissionCodes)
        {
            return helper.HtmlLink(innerHtml, actionName, null, null, htmlAttributes, permissionCodes);
        }

        /// <summary>
        /// Returns an anchor element (a element) for the specified inner HTML, action, route values, and HTML attributes.
        /// </summary>
        /// <param name="helper">The HTML helper instance that this method extends.</param>
        /// <param name="innerHtml">The inner HTML element of the anchor element.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="routeValues">An object that contains the parameters for a route. The parameters are retrieved through reflection by examining the properties of the object. The object is typically created by using object initializer syntax.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <param name="permissionCodes">permissions that have to be checked to show the element.</param>
        /// <returns>An anchor element (a element) if current user is authenticated and has the specified permission codes.</returns>
        public static MvcHtmlString HtmlLink(this HtmlHelper helper, string innerHtml, string actionName, object routeValues, object htmlAttributes, params string[] permissionCodes)
        {
            return helper.HtmlLink(innerHtml, actionName, null, routeValues, htmlAttributes, permissionCodes);
        }

        /// <summary>
        /// Returns an anchor element (a element) for the specified inner HTML, action, controller, route values, and HTML attributes.
        /// </summary>
        /// <param name="helper">The HTML helper instance that this method extends.</param>
        /// <param name="innerHtml">The inner HTML element of the anchor element.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">An object that contains the parameters for a route. The parameters are retrieved through reflection by examining the properties of the object. The object is typically created by using object initializer syntax.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <param name="permissionCodes">permissions that have to be checked to show the element.</param>
        /// <returns>An anchor element (a element) if current user is authenticated and has the specified permission codes.</returns>
        public static MvcHtmlString HtmlLink(this HtmlHelper helper, string innerHtml, string actionName, string controllerName, object routeValues, object htmlAttributes, params string[] permissionCodes)
        {
            return helper.HtmlLink(innerHtml, actionName, controllerName, null, null, null, routeValues, htmlAttributes, permissionCodes);
        }

        /// <summary>
        /// Returns an anchor element (a element) for the specified inner HTML, action, controller, protocol, host name, URL fragment, route values, and HTML attributes.
        /// </summary>
        /// <param name="helper">The HTML helper instance that this method extends.</param>
        /// <param name="innerHtml">The inner HTML element of the anchor element.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="protocol">The protocol for the URL, such as "http" or "https".</param>
        /// <param name="hostName">The host name for the URL.</param>
        /// <param name="fragment">The URL fragment name (the anchor name).</param>
        /// <param name="routeValues">An object that contains the parameters for a route. The parameters are retrieved through reflection by examining the properties of the object. The object is typically created by using object initializer syntax.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <param name="permissionCodes">permissions that have to be checked to show the element.</param>
        /// <returns>An anchor element (a element) if current user is authenticated and has the specified permission codes.</returns>
        public static MvcHtmlString HtmlLink(this HtmlHelper helper, string innerHtml, string actionName, string controllerName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes, params string[] permissionCodes)
        {
            RouteValueDictionary parameters = HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues);
            RouteValueDictionary attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            return helper.HtmlLink(innerHtml, actionName, controllerName, protocol, hostName, fragment, parameters, attributes, permissionCodes);
        }

        /// <summary>
        /// Returns an anchor element (a element) for the specified inner HTML, action, controller, protocol, host name, URL fragment, route values as a route value dictionary, and HTML attributes as a dictionary.
        /// </summary>
        /// <param name="helper">The HTML helper instance that this method extends.</param>
        /// <param name="innerHtml">The inner HTML element of the anchor element.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="protocol">The protocol for the URL, such as "http" or "https".</param>
        /// <param name="hostName">The host name for the URL.</param>
        /// <param name="fragment">The URL fragment name (the anchor name).</param>
        /// <param name="routeValues">An object that contains the parameters for a route.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <param name="permissionCodes">permissions that have to be checked to show the element.</param>
        /// <returns>An anchor element (a element) if current user is authenticated and has the specified permission codes.</returns>
        public static MvcHtmlString HtmlLink(this HtmlHelper helper, string innerHtml, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes, params string[] permissionCodes)
        {
            return GenerateLinkInternal(helper.ViewContext.RequestContext, helper.RouteCollection, innerHtml, null, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes, permissionCodes);
        }

        #endregion

        public static MvcHtmlString PopoverConfirmLink(this HtmlHelper helper, string title, string innerHtml, object routeValues, object htmlAttributes, params string[] permissionCodes)
        {
            var parameters = HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues);
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            attributes.Add("class", "popper");
            attributes.Add("data-toggle", "popover");
            attributes.Add("data-original-title", title);
            attributes.Add("onclick", "return false;");

            return Link(helper, innerHtml, "GetConfirmationPopover", "shared", parameters, attributes, permissionCodes);
        }

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
            return PopoverConfirmLink(helper, linkInnerHtml, confirmationTitle, confirmationMessage, actionName, controllerName, Util.EncryptRouteValues(routeValues), ajaxOptions, htmlAttributes, onlyForRoles);
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
        public static MvcHtmlString AcionLinkWithConfirm(this HtmlHelper helper, string linkInnerHtml, string confirmationTitle, string confirmationMessage, string actionName, string controllerName = null, object routeValues = null, object htmlAttributes = null, string onlyForRoles = null)
        {
            RouteValueDictionary customAttributes = AddConfirmationAttributes(htmlAttributes, confirmationTitle, confirmationMessage);
            RouteValueDictionary d;
            if (routeValues is RouteValueDictionary)
                d = routeValues as RouteValueDictionary;
            else
                d = new RouteValueDictionary(routeValues);
            return GenerateLinkInternal(helper.ViewContext.RequestContext, helper.RouteCollection, linkInnerHtml, null, actionName, controllerName, null, null, null, d, customAttributes, onlyForRoles);
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
        public static MvcHtmlString PopoverConfirmLink(this AjaxHelper helper, string linkInnerHtml, string confirmationTitle, string confirmationMessage, string actionName, string controllerName = null, object routeValues = null, AjaxOptions ajaxOptions = null, object htmlAttributes = null, string onlyForRoles = null)
        {
            RouteValueDictionary attributes = AddConfirmationAttributes(htmlAttributes, confirmationTitle, confirmationMessage);
            RouteValueDictionary parameters = HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues);

            ajaxOptions = (ajaxOptions ?? new AjaxOptions());
            if (string.IsNullOrWhiteSpace(ajaxOptions.OnBegin))
                ajaxOptions.OnBegin = "ReturnFalse";

            List<string> permissionCodes = new List<string>();
            if (onlyForRoles != null)
                permissionCodes.Add(onlyForRoles);

            return helper.Link(linkInnerHtml, actionName, controllerName, parameters, attributes, ajaxOptions, permissionCodes.ToArray());
        }

        public static MvcHtmlString PopoverConfirmLink(this AjaxHelper helper, string innerHtml, string actionName, string controllerName, object routeValues, object htmlAttributes, AjaxOptions ajaxOptions, ConfirmInfo confirmInfo, params string[] permissionCodes)
        {
            return null;
        }

        public static MvcHtmlString HtmlLink(this AjaxHelper helper, string innerHtml, string actionName, string controllerName, RouteValueDictionary routeValues, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes, params string[] permissionCodes)
        {
            string targetUrl = UrlHelper.GenerateUrl(null, actionName, controllerName, null, null, null, routeValues, helper.RouteCollection, helper.ViewContext.RequestContext, true);

            return MvcHtmlString.Create(GenerateLink(helper, innerHtml, targetUrl, ajaxOptions, htmlAttributes));
        }

        public static MvcHtmlString PopoverConfirmLink(this AjaxHelper helper, string innerHtml, string actionName, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes, ConfirmInfo confirmInfo, params string[] permissionCodes)
        {
            RouteValueDictionary parameters = HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues);
            RouteValueDictionary attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);


            ajaxOptions.ToUnobtrusiveHtmlAttributes();
            //confirmInfo.PopulateAjaxOptions(ajaxOptions);
            //confirmInfo.PopulateParameters(parameters);

            HtmlHelper htmlHelper = new HtmlHelper(helper.ViewContext, helper.ViewDataContainer, helper.RouteCollection);

            return htmlHelper.Link(innerHtml, actionName, null, parameters, attributes, permissionCodes);
        }

        private static MvcHtmlString GenerateEncryptedLinkInternal(RequestContext requestContext, RouteCollection routeCollection, string innerHtml, string routeName, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes, params string[] permissionCodes)
        {
            RouteValueDictionary parameters = Util.EncryptRouteValues(routeValues);

            return GenerateLinkInternal(requestContext, routeCollection, innerHtml, routeName, actionName, controllerName, protocol, hostName, fragment, parameters, htmlAttributes, permissionCodes);
        }

        private static MvcHtmlString GenerateLinkInternal(RequestContext requestContext, RouteCollection routeCollection, string innerHtml, string routeName, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes, params string[] permissionCodes)
        {
            if (!ChallengePermissionCode(permissionCodes))
                return MvcHtmlString.Empty;

            string url = UrlHelper.GenerateUrl(routeName, actionName, controllerName, protocol, hostName, fragment, routeValues, routeCollection, requestContext, false);

            TagBuilder tagBuilder = new TagBuilder("a")
            {
                InnerHtml = (!String.IsNullOrEmpty(innerHtml)) ? innerHtml : string.Empty
            };
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("href", url);

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }

        [Obsolete()]
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

        private static string GenerateLink(AjaxHelper ajaxHelper, string innerHtml, string targetUrl, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
        {
            TagBuilder tag = new TagBuilder("a")
            {
                InnerHtml = (!string.IsNullOrEmpty(innerHtml)) ? innerHtml : string.Empty
            };

            tag.MergeAttributes(htmlAttributes);
            tag.MergeAttribute("href", targetUrl);

            if (ajaxHelper.ViewContext.UnobtrusiveJavaScriptEnabled)
            {
                tag.MergeAttributes(ajaxOptions.ToUnobtrusiveHtmlAttributes());
            }
            else
            {
                throw new InvalidOperationException("Unobtrusive Java Script must be enabled.");
            }

            return tag.ToString(TagRenderMode.Normal);
        }

        #region Confirmable and Encrypted (Ajax / HTML) Action Link

        /// <summary>
        /// Returns a confirmable and encrypted HREF anchor element (a element) that ask user for confirmation before AJAX submit to server.
        /// </summary>
        /// <param name="helper">The HTML helper instance that this method extends.</param>
        /// <param name="innerHtml">The inner HTML tag of the anchor element.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <param name="permissionCodes">permissions that have to be checked to show the element.</param>
        /// <returns>A confirmable and encrypted HREF anchor element (a element) if current user is authenticated and has the specified permission codes.</returns>
        public static ConfirmableLink ConfirmableLink(this AjaxHelper helper, string innerHtml, object htmlAttributes, params string[] permissionCodes)
        {
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            return ConfirmableLink(helper, innerHtml, attributes, permissionCodes);
        }

        /// <summary>
        /// Returns a confirmable and encrypted HREF anchor element (a element) that ask user for confirmation before AJAX submit to server.
        /// </summary>
        /// <param name="helper">The HTML helper instance that this method extends.</param>
        /// <param name="innerHtml">The inner HTML tag of the anchor element.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <param name="permissionCodes">permissions that have to be checked to show the element.</param>
        /// <returns>A confirmable and encrypted HREF anchor element (a element) if current user is authenticated and has the specified permission codes.</returns>
        public static ConfirmableLink ConfirmableLink(this AjaxHelper helper, string innerHtml, IDictionary<string, object> htmlAttributes, params string[] permissionCodes)
        {
            var confirmableLink = new ConfirmableLink(helper);
            confirmableLink.Link(innerHtml, htmlAttributes, permissionCodes);

            return confirmableLink;
        }

        /// <summary>
        /// Returns a confirmable and encrypted HREF anchor element (a element) that as user for confirmation before regular submit to server.
        /// </summary>
        /// <param name="helper">The HTML helper instance that this method extends.</param>
        /// <param name="innerHtml">The inner HTML tag of the anchor element.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <param name="permissionCodes">permissions that have to be checked to show the element.</param>
        /// <returns>A confirmable and encrypted HREF anchor element (a element) if current user is authenticated and has the specified permission codes.</returns>
        public static ConfirmableLink ConfirmableLink(this HtmlHelper helper, string innerHtml, object htmlAttributes, params string[] permissionCodes)
        {
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            return ConfirmableLink(helper, innerHtml, attributes, permissionCodes);
        }

        /// <summary>
        /// Returns a confirmable and encrypted HREF anchor element (a element) that as user for confirmation before regular submit to server.
        /// </summary>
        /// <param name="helper">The HTML helper instance that this method extends.</param>
        /// <param name="innerHtml">The inner HTML tag of the anchor element.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <param name="permissionCodes">permissions that have to be checked to show the element.</param>
        /// <returns>A confirmable and encrypted HREF anchor element (a element) if current user is authenticated and has the specified permission codes.</returns>
        public static ConfirmableLink ConfirmableLink(this HtmlHelper helper, string innerHtml, IDictionary<string, object> htmlAttributes, params string[] permissionCodes)
        {
            var confirmableLink = new ConfirmableLink(helper);
            confirmableLink.Link(innerHtml, htmlAttributes, permissionCodes);

            return confirmableLink;
        }

        public static PopoverLink Dialog(this HtmlHelper helper, string innerHtml, object htmlAttributes, params string[] permissionCodes)
        {
            var popoverLink = new PopoverLink(helper);
            popoverLink.Link(innerHtml, htmlAttributes, permissionCodes);

            return popoverLink;
        }
        #endregion
    }
}