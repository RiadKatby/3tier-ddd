using RefactorName.WebApp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace RefactorName.WebApp.Helpers
{
    /// <summary>
    /// Provide confirmation (Popover, or Modal) capability before submit an action to specific URL
    /// </summary>
    public class ConfirmableLink : IHtmlString
    {
        private AjaxHelper ajaxHelper;
        private HtmlHelper htmlHelper;

        private string innerHtml;
        private IDictionary<string, object> htmlAttributes;
        private string[] permissionCodes;
        private object controller;
        private string actionName;
        RouteValueDictionary attributes;

        private ConfirmInfo confirmInfo;

        /// <summary>
        /// Initializes a new instance of the ConfirmableLink class that generate Ajax Positive Button.
        /// </summary>
        /// <param name="helper"></param>
        public ConfirmableLink(AjaxHelper helper)
        {
            ajaxHelper = helper;
            controller = helper.ViewContext.RouteData.Values["controller"];

        }

        /// <summary>
        /// Initializes a new instance of the ConfirmableLink class that generate HTML Positive Button.
        /// </summary>
        /// <param name="helper"></param>
        public ConfirmableLink(HtmlHelper helper)
        {
            htmlHelper = helper;
            controller = helper.ViewContext.RouteData.Values["controller"];
        }

        internal void Link(string innerHtml, IDictionary<string, object> htmlAttributes, params string[] permissionCodes)
        {
            this.innerHtml = innerHtml;
            this.htmlAttributes = htmlAttributes;
            this.permissionCodes = permissionCodes;
        }

        /// <summary>
        /// Specify information of confirm dialog and positive action information.
        /// </summary>
        /// <param name="actionName">action name of positive button.</param>
        /// <param name="controllerName">controller name of positive action</param>
        /// <param name="routeValues">route values of positive action.</param>
        /// <param name="title">Title text of confirmation dialog.</param>
        /// <param name="message">message text of confirmation dialog.</param>
        /// <returns></returns>
        public ConfirmableLink ConfirmedByPopover(string actionName, string controllerName, object routeValues, string title, string message)
        {
            confirmInfo = new ConfirmInfo(title, message);

            var parameters = Util.EncryptRouteValues(routeValues);

            RouteValueDictionary routeData = new RouteValueDictionary();
            routeData.Add(ConfirmInfo.ActionNameKey, actionName);
            routeData.Add(ConfirmInfo.ControllerNameKey, controllerName ?? controller);
            routeData.Add(ConfirmInfo.QueryStringKey, parameters["q"]);

            attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(this.htmlAttributes);
            attributes.Add("title", confirmInfo.Title);
            attributes.Add("data-toggle", "popover");
            attributes.Add("data-placement", "top");
            attributes.Add("onClick", "return false;");
            //attributes.Add("data-trigger", "focus");


            this.actionName = this.htmlHelper == null ? "GetAjaxConfirmationPopover" : "GetHtmlConfirmationPopover";

            confirmInfo.Url = StringEncrypter.UrlEncrypter.Encrypt(routeData.ToQueryString().ToString());

            return this;
        }

        public ConfirmableLink ConfirmedByModal(string actionName, string controllerName, object routeValues, string title, string message)
        {
            confirmInfo = new ConfirmInfo(title, message);

            var parameters = Util.EncryptRouteValues(routeValues);

            RouteValueDictionary routeData = new RouteValueDictionary();
            routeData.Add(ConfirmInfo.ActionNameKey, actionName);
            routeData.Add(ConfirmInfo.ControllerNameKey, controllerName ?? controller);
            routeData.Add(ConfirmInfo.QueryStringKey, parameters["q"]);

            attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(this.htmlAttributes);
            attributes.Add("title", confirmInfo.Title);
            attributes.Add("data-toggle", "modal");
            attributes.Add("onClick", "GetModal(this);");
            attributes.Add("data-target", "#popupModal");

            this.actionName = this.htmlHelper == null ? "GetAjaxConfirmationModal" : "GetHtmlConfirmationModal";
            confirmInfo.Url = StringEncrypter.UrlEncrypter.Encrypt(routeData.ToQueryString().ToString());
            return this;
            //return htmlHelper.HtmlLink(innerHtml, "GetModalConfirmationPopover", "Shared", null, null, null, confirmInfo.ToRouteValues(), attributes, permissionCodes);
        }

        public ConfirmableLink ConfirmedByPopover(string actionName, string controllerName, RouteValueDictionary routeValues, string title, string message, string positiveButton, string negativeButton, ButtonStyle positiveButtonStyle)
        {
            confirmInfo = new ConfirmInfo(title, message, positiveButton, negativeButton, positiveButtonStyle);

            routeValues = Util.EncryptRouteValues(routeValues);
            routeValues.Add(ConfirmInfo.ActionNameKey, actionName);
            routeValues.Add(ConfirmInfo.ControllerNameKey, controllerName);

            confirmInfo.Url = StringEncrypter.UrlEncrypter.Encrypt(routeValues.ToQueryString().ToString());

            return this;
        }

        public ConfirmableLink RequestOptions(string httpMethod, InsertionMode insertionMode, string loadingElementId, string updateTargetId, string onBegin, string onComplete, string onFailure, string onSuccess)
        {
            AjaxOptions ajaxOptions = new AjaxOptions
            {
                HttpMethod = httpMethod,
                InsertionMode = insertionMode,
                LoadingElementId = loadingElementId,
                UpdateTargetId = updateTargetId,
                OnBegin = onBegin,
                OnComplete = onComplete,
                OnFailure = onFailure,
                OnSuccess = onSuccess
            };

            confirmInfo.Attributes = ajaxOptions
                .ToQueryString()
                .EncryptAndUrlEncode();

            return this;
        }

        public ConfirmableLink RequestOptions(string httpMethod, string updateTargetId, string loadingElementId)
        {
            return RequestOptions(httpMethod, InsertionMode.Replace, loadingElementId, updateTargetId, null, null, null, null);
        }

        public string ToHtmlString()
        {
            var htmlHelper = this.htmlHelper ?? new HtmlHelper(ajaxHelper.ViewContext, ajaxHelper.ViewDataContainer, ajaxHelper.RouteCollection);
            return htmlHelper.HtmlLink(innerHtml, actionName, "Shared", null, null, null, confirmInfo.ToRouteValues(), attributes, permissionCodes).ToHtmlString();
        }
    }
}