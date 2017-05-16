using RefactorName.Core;
using RefactorName.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace RefactorName.WebApp
{
    public partial class BaseController
    {
        internal const string SnackbarsKey = "__snackbarz";
        internal const string CurrentControllerInstanceKey = "__CurrentControllerInstance";
        internal const string SnackbarMessageHeaderKey = "dev-fw-message";
        internal const string SnackbarMessageTypeHeaderKey = "dev-fw-message-type";
        internal const string SnackbarMessageTimeoutHeaderKey = "dev-fw-message-timeout";

        /// <summary>
        /// Push new <see cref="SnackbarType.danger"/> message to User Snackbar Area.
        /// </summary>
        /// <param name="message"><see cref="SnackbarType.danger"/> message text.</param>
        public void ShowDangerSnackbar(string message)
        {
            LayoutModel.Snackbars.Add(new SnackbarViewModel(message, SnackbarType.danger, Settings.Provider.SnackbarDangerMessageTimeout));
        }

        /// <summary>
        /// Push new <see cref="SnackbarType.success"/> message to User Snackbar Area.
        /// </summary>
        /// <param name="message"><see cref="SnackbarType.success"/> message text.</param>
        public void ShowSuccessSnackbar(string message)
        {
            LayoutModel.Snackbars.Add(new SnackbarViewModel(message, SnackbarType.success, Settings.Provider.SnackbarSuccessMessageTimeout));
        }

        /// <summary>
        /// Push new <see cref="SnackbarType.info"/> message to User Snackbar Area.
        /// </summary>
        /// <param name="message"><see cref="SnackbarType.info"/> message text.</param>
        public void ShowInfoSnackbar(string message)
        {
            LayoutModel.Snackbars.Add(new SnackbarViewModel(message, SnackbarType.info, Settings.Provider.SnackbarInfoMessageTimeout));
        }

        /// <summary>
        /// Push new <see cref="SnackbarType.warning"/> message to User Snackbar Area.
        /// </summary>
        /// <param name="message"><see cref="SnackbarType.warning"/> message text.</param>
        public void ShowWarningSnackbar(string message)
        {
            LayoutModel.Snackbars.Add(new SnackbarViewModel(message, SnackbarType.warning, Settings.Provider.SnackbarWarningMessageTimeout));
        }

        #region Ajax OnFaild Message Helper Methods

        /// <summary>
        /// Return <see cref="SnackbarType.warning"/> message only to client side to show it in SnackbarArea. 
        /// </summary>
        /// <param name="message">text formated message that will shown to user.</param>
        /// <param name="values">values of formated message.</param>
        /// <returns><see cref="JsonResult"/> object that contains the warning message.</returns>
        /// <exception cref="InvalidOperationException">When Http Request is not AJAX.</exception>
        public ActionResult WarningSnackbar(string message, params object[] values)
        {
            return MessageHelper(SnackbarType.warning, message, HttpStatusCode.BadRequest, values);
        }

        /// <summary>
        /// Returns <see cref="SnackbarType.info"/> message only to client side to show it in SnackbarArea.
        /// </summary>
        /// <param name="message">text formated message that will shown to user.</param>
        /// <param name="values">values of formated message.</param>
        /// <returns><see cref="JsonResult"/> object that contains the info message.</returns>
        /// <exception cref="InvalidOperationException">When Http Request is not AJAX.</exception>
        public ActionResult InfoSnackbar(string message, params object[] values)
        {
            return MessageHelper(SnackbarType.info, message, HttpStatusCode.BadRequest, values);
        }

        /// <summary>
        /// Returns <see cref="SnackbarType.danger"/> message only to client side to show it in SnackbarArea.
        /// </summary>
        /// <param name="message">text formated message that will shown to user.</param>
        /// <param name="values">values of formated message.</param>
        /// <returns><see cref="JsonResult"/> object that contains the danger message.</returns>
        /// <exception cref="InvalidOperationException">When Http Request is not AJAX.</exception>
        public ActionResult DangerSnackbar(string message, params object[] values)
        {
            return MessageHelper(SnackbarType.danger, message, HttpStatusCode.BadRequest, values);
        }

        /// <summary>
        /// Returns <see cref="SnackbarType.success"/> message only to client side to show it in SnackbarArea.
        /// </summary>
        /// <param name="message">text formated message that will shown to user.</param>
        /// <param name="values">values of formated message.</param>
        /// <returns><see cref="JsonResult"/> object that contains the success message.</returns>
        /// <exception cref="InvalidOperationException">When Http Request is not AJAX.</exception>
        public ActionResult SuccessSnackbar(string message, params object[] values)
        {
            return MessageHelper(SnackbarType.success, message, HttpStatusCode.BadRequest, values);
        }

        protected ActionResult MessageHelper(SnackbarType messageType, string message, HttpStatusCode statusCode, params object[] values)
        {
            if (!HttpContext.Request.IsAjaxRequest())
                throw new InvalidOperationException("The Request must be Ajax to call these method.");

            var errorMessages = from x in ModelState
                                where x.Value.Errors.Any()
                                select new
                                {
                                    key = x.Key,
                                    value = x.Value.Errors.First().ErrorMessage
                                };

            Response.StatusCode = (int)statusCode;
            Response.StatusDescription = message;

            Response.AddHeader(SnackbarMessageHeaderKey, Server.HtmlEncode(message));

            return Json(new { message = string.Format(message, values), messageType = messageType.ToString(), modelStateErrors = errorMessages.ToArray() }, JsonRequestBehavior.AllowGet);
        }

        #endregion Ajax OnFaild Message Helper Methods
    }
}