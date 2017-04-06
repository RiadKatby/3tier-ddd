using System.Web.Mvc;

namespace RefactorName.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {            
            return View();
        }

    }
}
