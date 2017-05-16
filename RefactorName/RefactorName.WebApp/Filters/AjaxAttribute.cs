using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RefactorName.WebApp.Filters
{
    public class AjaxAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var response = filterContext.RequestContext.HttpContext.Response;
                response.RedirectToRoute(new RouteValueDictionary {
                        { "Controller",  "Shared"},
                        { "Action", "Error" }
                });
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}