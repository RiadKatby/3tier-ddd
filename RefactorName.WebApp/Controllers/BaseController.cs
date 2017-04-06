using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RefactorName.Web.Controllers
{
    public class BaseController : Controller, IDisposable
    {
        public JsonResult JsonErrorMessage(string errorMessage)
        {
            var response = HttpContext.Response;
            response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            response.StatusDescription = errorMessage;

            return Json(new { message = errorMessage }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsonErrorMessage(string errorMessage, ModelStateDictionary modelState)
        {
            var response = HttpContext.Response;
            response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            response.StatusDescription = errorMessage;

            var modelStateErrors = modelState.Where(m => m.Value.Errors.Any()).ToList();
            //to avoid circular
            var cleanModelStateErrors = modelStateErrors.Select(m => new
            {
                key = m.Key,
                value = m.Value.Errors.FirstOrDefault().ErrorMessage
            });

            return Json(new { message = errorMessage, modelStateErrors = cleanModelStateErrors }, JsonRequestBehavior.AllowGet);
        }


        public void AddMCIMessage(string message, MCIMessageType type = MCIMessageType.Info, int timeout = 5)
        {
            MCIAlert.AddMCIMessage(this, message, type, timeout);
        }

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
                        AddMCIMessage(userMessage, MCIMessageType.Danger, 15);
                        return View(toReturnViewName, model);
                    }
                    else
                        return Content(errorBody);
                }
                else
                    return JsonErrorMessage(userMessage);
            }
            return Content(errorBody);
        }

        protected ActionResult RedirectwithMCIMessage(string redirectTo, string message = "", MCIMessageType messageType = MCIMessageType.Info, int messageTimeOut = 8)
        {
            if (!string.IsNullOrWhiteSpace(message))
                AddMCIMessage(message, messageType, messageTimeOut);

            if (HttpContext.Request.IsAjaxRequest())
                return Json(new { url = redirectTo }, JsonRequestBehavior.AllowGet);
            else
                return Redirect(redirectTo);
        }


        void IDisposable.Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}