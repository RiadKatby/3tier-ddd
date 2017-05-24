using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RefactorName.WebApp.Helpers
{
    public class PopoverLink : IHtmlString
    {
        private HtmlHelper htmlHelper;

        private string innerHtml;
        private object htmlAttributes;
        private string[] permissionCodes;
        //private object controller;
        private string actionName;
        private string controllerName;
        RouteValueDictionary attributes;

        RouteValueDictionary routeValue;

        public PopoverLink(HtmlHelper helper)
        {
            htmlHelper = helper;
        }

        internal void Link(string innerHtml, object htmlAttributes, params string[] permissionCodes)
        {
            this.innerHtml = innerHtml;
            this.htmlAttributes = htmlAttributes;
            this.permissionCodes = permissionCodes;
        }
        public PopoverLink ViewByPopover(string actionName, string controllerName, object routeValues)
        {
            routeValue = Util.EncryptRouteValues(routeValues);

            attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(this.htmlAttributes);
            attributes.Add("data-toggle", "popover");
            attributes.Add("data-placement", "top");
            attributes.Add("onClick", "return false;");
            //attributes.Add("data-trigger", "focus");
            this.controllerName = controllerName ?? htmlHelper.ViewContext.RouteData.Values["controller"].ToString();
            this.actionName = actionName;

            return this;
        }

        public string ToHtmlString()
        {
            return htmlHelper.HtmlLink(innerHtml, actionName, controllerName, null, null, null, routeValue, attributes, permissionCodes).ToHtmlString();
        }
    }
}