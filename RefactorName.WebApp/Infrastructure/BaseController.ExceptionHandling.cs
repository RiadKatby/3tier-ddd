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
    partial class BaseController
    {
        const string ArgumentNullErrorTitle = "Incorrect Method Call";
        const string ArgumentNullErrorDescription = "You Pass null to method that doesn't accept it.";
        const string ArgumentOutOfRangeErrorTitle = "Incorrect Method Call";
        const string ArgumentOutOfRangeErrorDescription = "You pass our of range value to method that doesn't accept it.";
        const string ArgumentErrorTilte = "Incorrect Method Call";
        const string ArgumentErrorDescription = "You pass incorrect values to method.";

        const string UnknownErrorTilte = "Unknown Error";
        const string UnknownErrorDescription = "An unknown error has occurred.";

        public const string LayoutModelKey = "__LayoutModel";

        /// <summary>
        /// Gets __Layout view model info.
        /// </summary>
        public LayoutViewModel LayoutModel { get; private set; }

        public BaseController()
        {
            LayoutModel = new LayoutViewModel();
        }

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var actionResult = filterContext.Result;

            if (actionResult is RedirectResult || actionResult is RedirectToRouteResult || actionResult is JsonResult)
                TempData[LayoutModelKey] = LayoutModel;
            else
            {
                var viewResult = filterContext.Result as ViewResult;
                if (viewResult != null)
                    viewResult.ViewBag.LayoutModel = LayoutModel;
            }

            base.OnResultExecuting(filterContext);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            // When exception thrown inside ActionMethod OnActionResultExecuting will not fired
            // so LayoutModel must be set here as well.
            var actionResult = filterContext.Result;
            if (actionResult is RedirectResult || actionResult is RedirectToRouteResult)
                TempData[LayoutModelKey] = LayoutModel;
            else
                ViewBag.LayoutModel = LayoutModel;

            var controllerName = filterContext.RouteData.Values["controller"].ToString();
            var actionName = filterContext.RouteData.Values["action"].ToString();

            ErrorModel model = new ErrorModel(filterContext.Exception, controllerName, actionName);
            ActionResult viewResult = null;

            if (TryHandleNotFound(filterContext, model))
            {
                Response.StatusCode = 404;

                viewResult = Request.IsAjaxRequest() ?
                    MessageHelper(SnackbarType.danger, model.Exception.Message, (HttpStatusCode)Response.StatusCode) :
                    View("PageNotFound", model);
            }

            if (TryHandleUnauthorized(filterContext, model))
            {
                Response.StatusCode = 401;

                viewResult = Request.IsAjaxRequest() ?
                    MessageHelper(SnackbarType.danger, model.Exception.Message, (HttpStatusCode)Response.StatusCode) :
                    View("Unauthorized", model);
            }

            if (TryHandleBadRequest(filterContext, model))
            {
                Response.StatusCode = 400;

                viewResult = Request.IsAjaxRequest() ?
                    MessageHelper(SnackbarType.danger, model.Exception.Message, (HttpStatusCode)Response.StatusCode) :
                    View("Error", model);
            }

            if (TryHandleInternalError(filterContext, model))
            {
                Response.StatusCode = 500;

                viewResult = Request.IsAjaxRequest() ?
                    MessageHelper(SnackbarType.danger, model.Exception.Message, (HttpStatusCode)Response.StatusCode) :
                    View("Error", model);
            }

            if (viewResult != null)
            {
                Response.TrySkipIisCustomErrors = true;
                filterContext.Result = viewResult;
            }
        }

        protected virtual bool TryHandleUnauthorized(ExceptionContext context, ErrorModel model)
        {
            var permissionEx = context.Exception as PermissionException;
            if (permissionEx == null)
                return false;

            // to prevent exception pop-up to Global.asax Application_Error
            context.ExceptionHandled = true;

            model.ErrorTitle = permissionEx.Title;
            model.Description = permissionEx.Description;
            model.UserName = permissionEx.UserName;
            model.PermissionCode = permissionEx.PermissionCode;

            return true;
        }

        protected virtual bool TryHandleNotFound(ExceptionContext context, ErrorModel model)
        {
            var notFoundEx = context.Exception as EntityNotFoundException;
            if (notFoundEx == null)
                return false;

            Tracer.Log.EntityNotFound(notFoundEx.EntityName, notFoundEx.EntityID, notFoundEx.ToString());

            // to prevent exception pop-up to Global.asax Application_Error
            context.ExceptionHandled = true;

            model.ErrorTitle = notFoundEx.Title;
            model.Description = notFoundEx.Description;
            model.EntityName = notFoundEx.EntityName;
            model.EntityId = notFoundEx.EntityID;

            return true;
        }

        protected virtual bool TryHandleBadRequest(ExceptionContext context, ErrorModel model)
        {
            var nullEx = context.Exception as ArgumentNullException;
            if (nullEx != null)
            {
                Tracer.Log.ArgumentNull(nullEx.ParamName, nullEx.ToString());

                // to prevent exception pop-up to Global.asax Application_Error
                context.ExceptionHandled = true;

                model.ErrorTitle = ArgumentNullErrorTitle;
                model.Description = ArgumentNullErrorDescription;
                model.ParamName = nullEx.ParamName;

                return true;
            }

            var rangEx = context.Exception as ArgumentOutOfRangeException;
            if (rangEx != null)
            {
                Tracer.Log.ArgumentOutOfRange(rangEx.ParamName, rangEx.ActualValue, rangEx.ToString());

                // to prevent exception pop-up to Global.asax Application_Error
                context.ExceptionHandled = true;

                model.ErrorTitle = ArgumentOutOfRangeErrorTitle;
                model.Description = ArgumentOutOfRangeErrorDescription;
                model.ParamName = rangEx.ParamName;
                model.ParamValue = rangEx.ActualValue;

                return true;
            }


            var argEx = context.Exception as ArgumentException;
            if (argEx != null)
            {
                Tracer.Log.Argument(argEx.ParamName, argEx.ToString());

                // to prevent exception pop-up to Global.asax Application_Error
                context.ExceptionHandled = true;

                model.ErrorTitle = ArgumentErrorTilte;
                model.Description = ArgumentErrorDescription;

                return true;
            }

            return false;
        }

        protected virtual bool TryHandleInternalError(ExceptionContext context, ErrorModel model)
        {
            var repEx = context.Exception as RepositoryException;
            if (repEx != null)
            {
                Tracer.Log.RepositoryFailure(repEx.EntityName, repEx.ErrorType.ToString(), repEx.ToString());

                // to prevent exception pop-up to Global.asax Application_Error
                context.ExceptionHandled = true;

                model.ErrorTitle = repEx.Title;
                model.Description = repEx.Description;
                model.EntityName = repEx.EntityName;
                model.ErrorType = repEx.ErrorType.ToString();

                return true;
            }


            var ex = context.Exception as Exception;
            if (ex != null)
            {
                Tracer.Log.Failure(ex.ToString());

                // to prevent exception pop-up to Global.asax Application_Error
                context.ExceptionHandled = true;

                model.ErrorTitle = UnknownErrorTilte;
                model.Description = UnknownErrorDescription;

                return true;
            }

            return false;
        }
    }
}