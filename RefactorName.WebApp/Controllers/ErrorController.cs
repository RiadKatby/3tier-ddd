using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RefactorName.WebApp.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult InternalError()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = 404;

            return View();
        }

        public ActionResult Permission()
        {
            return View();
        }

    }
}