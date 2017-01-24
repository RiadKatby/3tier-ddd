using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RefactorName.WebApp.Controllers
{
    public class WorkflowController : Controller
    {
        // GET: Workflow
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateProcss()
        {
            return View();
        }
    }
}