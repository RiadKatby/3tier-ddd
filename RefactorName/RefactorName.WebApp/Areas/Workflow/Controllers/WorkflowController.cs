using RefactorName.WebApp.Areas.Workflow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RefactorName.WebApp.Areas.Workflow.Controllers
{
    public class WorkflowController : Controller
    {
        // GET: Workflow/Workflow
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateProcess()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProcess(ProcessModel model)
        {
            return View();
        }
    }
}