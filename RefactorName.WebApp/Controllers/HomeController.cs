using RefactorName.Domain;
using System.Web.Mvc;
using RefactorName.WebApp.Helpers;
using System;

namespace RefactorName.WebApp.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }
    }
}
