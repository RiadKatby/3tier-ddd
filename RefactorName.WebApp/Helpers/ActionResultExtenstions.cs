using RefactorName.Core;
using RefactorName.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RefactorName.WebApp.Helpers
{
    public static class ActionResultExtenstions
    {
        /// <summary>
        /// Return <see cref="SnackbarType.info"/> Message within your View.
        /// </summary>
        /// <param name="actionResult"></param>
        /// <param name="message"><see cref="SnackbarType.info"/> massage text.</param>
        /// <param name="values">values if message has place holder.</param>
        /// <returns></returns>
        public static ActionResult WithInfoSnackbar(this ActionResult actionResult, string message, params object[] values)
        {
            return WithSnackbar(actionResult, SnackbarType.info, Settings.Provider.SnackbarInfoMessageTimeout, message, values);
        }

        /// <summary>
        /// Return <see cref="SnackbarType.success"/> Message within your View.
        /// </summary>
        /// <param name="actionResult"></param>
        /// <param name="message"><see cref="SnackbarType.success"/> massage text.</param>
        /// <param name="values">values if message has place holder.</param>
        /// <returns></returns>
        public static ActionResult WithSuccessSnackbar(this ActionResult actionResult, string message, params object[] values)
        {
            return WithSnackbar(actionResult, SnackbarType.success, Settings.Provider.SnackbarSuccessMessageTimeout, message, values);
        }

        /// <summary>
        /// Return <see cref="SnackbarType.warning"/> Message within your View.
        /// </summary>
        /// <param name="actionResult"></param>
        /// <param name="message"><see cref="SnackbarType.warning"/> massage text.</param>
        /// <param name="values">values if message has place holder.</param>
        /// <returns></returns>
        public static ActionResult WithWarningSnackbar(this ActionResult actionResult, string message, params object[] values)
        {
            return WithSnackbar(actionResult, SnackbarType.warning, Settings.Provider.SnackbarWarningMessageTimeout, message, values);
        }

        /// <summary>
        /// Return <see cref="SnackbarType.danger"/> Message within your View.
        /// </summary>
        /// <param name="actionResult"></param>
        /// <param name="message"><see cref="SnackbarType.danger"/> massage text.</param>
        /// <param name="values">values if message has place holder.</param>
        /// <returns></returns>
        public static ActionResult WithDangerSnackbar(this ActionResult actionResult, string message, params object[] values)
        {
            return WithSnackbar(actionResult, SnackbarType.danger, Settings.Provider.SnackbarDangerMessageTimeout, message, values);
        }

        private static ActionResult WithSnackbar(ActionResult actionResult, SnackbarType messageType, int timeout, string format, params object[] values)
        {
            // retrieve the current instance of controller that I already stored it in HttpContext.Items while
            // controller factory class execute CreateController method.
            var controller = HttpContext.Current.Items[BaseController.CurrentControllerInstanceKey] as BaseController;

            if (controller.Request.IsAjaxRequest())
            {
                controller.Response.AddHeader(BaseController.SnackbarMessageHeaderKey, controller.Server.UrlEncode(string.Format(format, values)));
                controller.Response.AddHeader(BaseController.SnackbarMessageTypeHeaderKey, messageType.ToString());
                controller.Response.AddHeader(BaseController.SnackbarMessageTimeoutHeaderKey, timeout.ToString());
                return actionResult;
            }

            var snackBarModel = new SnackbarViewModel(string.Format(format, values), messageType, timeout);

            // add new SnackbarViewModel instance to the LayoutModel which will be read from _Layout.cshtml while rendered.
            controller.LayoutModel.Snackbars.Add(snackBarModel);

            return actionResult;
        }

        public static ActionResult WithModelState(this RedirectToRouteResult result, BaseController controller)
        {
            controller.TempData["__ModelState__"] = controller.ViewData.ModelState;

            return result;
        }

        public static ActionResult WhenAjax(this ActionResult actionResult)
        {
            RedirectToRouteResult result = actionResult as RedirectToRouteResult;

            if (result == null)
                throw new InvalidOperationException("Must be invoked with Redirect methods.");

            // retrieve the current instance of controller that I already stored it in HttpContext.Items while
            // controller factory class execute CreateController method.
            var controller = HttpContext.Current.Items[BaseController.CurrentControllerInstanceKey] as BaseController;

            if (!controller.Request.IsAjaxRequest())
                throw new InvalidOperationException("Request must be Ajax when invoking WhenAjax method.");

            UrlHelper helper = new UrlHelper(controller.Request.RequestContext);
            string url = helper.RouteUrl(result.RouteName, result.RouteValues);

            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new { url = url }
            };
        }
    }
}