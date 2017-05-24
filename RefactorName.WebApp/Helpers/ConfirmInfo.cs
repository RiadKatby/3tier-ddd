﻿using RefactorName.WebApp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RefactorName.WebApp.Helpers
{
    /// <summary>
    /// Specify the style of the button.
    /// </summary>
    public enum ButtonStyle
    {
        info, danger, primary, warning, success
    }

    /// <summary>
    /// Represents information of confirmation dialog that may shown to take user confirmation on specific action.
    /// </summary>
    [ModelBinder(typeof(AliasModelBinder))]
    public class ConfirmInfo
    {
        public const string ActionNameKey = "actionname";
        public const string ControllerNameKey = "controllername";
        public const string QueryStringKey = "q";
        public const string TitleKey = "title";
        public const string MessageKey = "message";
        public const string PositiveButtonKey = "positivebutton";
        public const string NegativeButtonKey = "negativebutton";
        public const string PositiveButtonStyleKey = "positivebuttonstyle";

        //public string ConfirmId { get; private set; }

        /// <summary>
        /// Gets title of Confirmation Dialog.
        /// </summary>
        [BindAlias("t")]
        public string Title { get; private set; }

        /// <summary>
        /// Gets message that ask the user about his/her confirmation.
        /// </summary>
        [BindAlias("m")]
        public string Message { get; private set; }

        /// <summary>
        /// Gets title of positive answer button.
        /// </summary>
        [BindAlias("pb")]
        public string PositiveButton { get; private set; }

        /// <summary>
        /// Gets the style of positive answer button.
        /// </summary>
        //[BindAlias("pbs")]
        public ButtonStyle PositiveButtonStyle { get; private set; }

        /// <summary>
        /// Gets title of negative answer button.
        /// </summary>
        [BindAlias("nb")]
        public string NegativeButton { get; private set; }

        public string Url { get; set; }
        public string Dialog { get; set; }
        public string Attributes { get; set; }

        #region Read from View

        /// <summary>
        /// Gets ActionName that will be called when Positive Button Clicked.
        /// </summary>
        public string ActionName { get; private set; }

        /// <summary>
        /// Gets ControllerName of Action that will be called when Positive Button Clicked.
        /// </summary>
        public string ControllerName { get; private set; }

        /// <summary>
        /// Gets encrypted chunk of parameters that will be based to ActionName.
        /// </summary>
        public string QueryString { get; private set; }

        /// <summary>
        /// Gets HTML Attributes that will be applied on the Positive Button.
        /// </summary>
        public RouteValueDictionary HtmlAttributes { get; private set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the ConfirmInfo class with default values.
        /// </summary>
        public ConfirmInfo(IDictionary<string, object> attributes)
        {
            Title = attributes[TitleKey].ToString();
            Message = attributes[MessageKey].ToString();
            PositiveButton = attributes[PositiveButtonKey].ToString();
            NegativeButton = attributes[NegativeButtonKey].ToString();
            PositiveButtonStyle = (ButtonStyle)Enum.Parse(typeof(ButtonStyle), attributes[PositiveButtonStyleKey].ToString());
        }

        /// <summary>
        /// Initializes new instance of ConfirmInfo with specific title, message and default values for rest properties.
        /// </summary>
        /// <param name="title">title of confirmation dialog box.</param>
        /// <param name="message">message that asks the user about his/her confirmation.</param>
        /// <param name="positiveButton">title of positive answer button.</param>
        /// <param name="negativeButton">title of negative answer button.</param>
        /// <param name="positiveStyle">style of positive answer button.</param>
        public ConfirmInfo(string title, string message)
        {
            Title = title;
            Message = message;
            PositiveButton = "نعم";
            NegativeButton = "لا";
            PositiveButtonStyle = ButtonStyle.primary;
        }

        /// <summary>
        /// Initializes a new instance of the ConfirmInfo class with specific title, message, positiveButton, negativeButton, positiveButtonStyle.
        /// </summary>
        /// <param name="title">Title text of confirmation dialog.</param>
        /// <param name="message">Message text of confirmation dialog.</param>
        /// <param name="positiveButton">Positive button caption.</param>
        /// <param name="negativeButton">Negative button caption.</param>
        /// <param name="positiveButtonStyle">Positive button style.</param>
        public ConfirmInfo(string title, string message, string positiveButton, string negativeButton, ButtonStyle positiveButtonStyle)
        {
            Title = title;
            Message = message;
            PositiveButton = positiveButton ?? "نعم";
            NegativeButton = negativeButton ?? "لا";
            PositiveButtonStyle = positiveButtonStyle;
        }

        public void PopulateAttribute(IDictionary<string, object> htmlAttributes)
        {
            HtmlAttributes = HtmlAttributes ?? new RouteValueDictionary(htmlAttributes);
            //HtmlAttributes["class"] += " mci-confirm";
            //HtmlAttributes["dialog-title"] = htmlAttributes[TitleKey].ToString();
            //HtmlAttributes["dialog-content"] = htmlAttributes[MessageKey].ToString();
            //HtmlAttributes["dialog-positiveButton"] = htmlAttributes[PositiveButtonKey].ToString();
            //HtmlAttributes["dialog-negativeButton"] = htmlAttributes[NegativeButtonKey].ToString();
            //HtmlAttributes["dialog-positiveButtonStyle"] = htmlAttributes[PositiveButtonStyleKey].ToString();
        }

        internal void PopulateUrl(IDictionary<string, object> urlParts)
        {
            ActionName = urlParts[ActionNameKey].ToString();
            ControllerName = urlParts[ControllerNameKey].ToString();
            QueryString = urlParts[QueryStringKey].ToString();
        }

        internal RouteValueDictionary ToRouteValues()
        {
            NameValueCollection dialog = HttpUtility.ParseQueryString("?");
            dialog.Add(TitleKey, Title);
            dialog.Add(MessageKey, Message);
            dialog.Add(PositiveButtonKey, PositiveButton);
            dialog.Add(NegativeButtonKey, NegativeButton);
            dialog.Add(PositiveButtonStyleKey, PositiveButtonStyle.ToString());

            var routeValues = new
            {
                anchor = Url,
                attributes = Attributes,
                dialog = StringEncrypter.UrlEncrypter.Encrypt(dialog.ToString())
            };

            return HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues);
        }
    }
}