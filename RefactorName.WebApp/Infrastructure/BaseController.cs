using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RefactorName.Core;
using RefactorName.WebApp.Models;
using System.Configuration;
using System.Threading;
using RefactorName.WebApp.Helpers;

namespace RefactorName.WebApp
{
    public partial class BaseController : Controller, IDisposable
    {
        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    filterContext.ExceptionHandled = true;
        //    string controller = filterContext.RouteData.Values["controller"].ToString();
        //    string action = filterContext.RouteData.Values["action"].ToString();
        //    Exception exception = filterContext.Exception;

        //    string errorTitle = string.Empty;
        //    if (exception is PermissionException)
        //        errorTitle = "Security Permission Required";

        //    ErrorModel model = new ErrorModel(exception, controller, action, errorTitle);
        //    var result = View("Error", model);
        //    result.ViewBag.LayoutModel = LayoutModel;
        //    filterContext.Result = result;
        //}

        //public JsonResult JsonErrorMessage(string errorMessage)
        //{
        //    var response = HttpContext.Response;
        //    response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
        //    response.StatusDescription = errorMessage;

        //    return Json(new { message = errorMessage }, JsonRequestBehavior.AllowGet);
        //}

        //public void AddMCIMessage(string message, MCIMessageType type = MCIMessageType.Info, int timeout = 5)
        //{
        //    MCIAlert.AddMCIMessage(this, message, type, timeout);
        //}

        protected ActionResult RegisterError(Exception ex, string actionName, int? objectID = null, string objectName = "", string toReturnViewName = "", object model = null, string userMessage = "عفواً .. حدث خطأ أثناء جلب  البيانات . الرجاء المحاولة لاحقاً.")
        {
            //
            //TODO: register Error in db here
            //

            string errorBody = string.Format("Exception: {1}.<br/>Exception Message:{0}.<br/>StackTrace: {2}.", ex.ToString(), ex.Message, ex.StackTrace);
            var iExeption = ex.InnerException;
            while (iExeption != null)
            {
                errorBody += string.Format("<br/>Inner Exception: {0}.", iExeption.Message);
                iExeption = iExeption.InnerException;
            }

            //developement mode
            if (Convert.ToBoolean(System.Web.Configuration.WebConfigurationManager.AppSettings["ExceptionDevelopmentMode"]))
                return Content(errorBody);

            // else return friendly message to the user
            if (!string.IsNullOrEmpty(userMessage))
            {
                if (!HttpContext.Request.IsAjaxRequest())
                {
                    if (!string.IsNullOrEmpty(toReturnViewName))
                    {
                        return View(toReturnViewName, model)
                            .WithDangerSnackbar(userMessage);
                    }
                    else
                        return Content(errorBody);
                }
                else
                    return DangerSnackbar(userMessage);
            }
            return Content(errorBody);
        }

        //protected ActionResult RedirectwithMCIMessage(string redirectTo, string message = "", MCIMessageType messageType = MCIMessageType.Info, int messageTimeOut = 8)
        //{
        //    if (!string.IsNullOrWhiteSpace(message))
        //        AddMCIMessage(message, messageType, messageTimeOut);

        //    if (HttpContext.Request.IsAjaxRequest())
        //        return Json(new { url = redirectTo }, JsonRequestBehavior.AllowGet);
        //    else
        //        return Redirect(redirectTo);
        //}

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var aspNetCookieName = ConfigurationManager.AppSettings["ASPNETCookieName"];
            var ctx = filterContext.HttpContext;
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                //if (Session["IsLoggedIn"] != null)
                //{
                //    if (ctx.Session[aspNetCookieName] == null && ctx.Request.Cookies[aspNetCookieName] != null)
                //    {
                //        ctx.Session[aspNetCookieName] = ctx.Request.Cookies[aspNetCookieName].Value;
                //    }
                //    if (ctx.Session[aspNetCookieName] != null)
                //    {
                //        if (ctx.Request.Cookies[aspNetCookieName].Value != ctx.Session[aspNetCookieName].ToString())
                //        {
                //            Session.Abandon();
                //            HttpContext.GetOwinContext().Get<ApplicationSignInManager>().AuthenticationManager.SignOut();
                //            Util.ClearCookiesAndSessions();
                //            filterContext.Result = RedirectToAction("Login", "Account")
                //                .WithDangerSnackbar("عفواً. حدث خطأ أثناء جلب البيانات. الرجاء المحاولة لاحقاً.");
                //        }
                //    }
                //}

                if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
                {
                    //var userIdentity = User.Identity as UserProfileIdentity;
                    //var sToken = Util.FetchTokenBySecureCookie();
                    //Trace.TraceInformation("Calim: {0} Token: {1}", userIdentity.GetClaimValue("Id"), EncryptionHelper.Decrypt(sToken));

                    //if (String.IsNullOrWhiteSpace(sToken)
                    //    || userIdentity.GetClaimValue("Id") != EncryptionHelper.Decrypt(sToken))
                    //    Response.Redirect("/account/logout");
                }
            }
            base.OnActionExecuting(filterContext);
        }

        protected bool IsRefresh()
        {
            return Convert.ToBoolean(RouteData.Values["IsRefreshed"]);
        }

        void IDisposable.Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}