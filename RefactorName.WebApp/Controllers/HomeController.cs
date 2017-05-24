using RefactorName.Domain;
using System.Web.Mvc;
using RefactorName.WebApp.Helpers;
using System;
using RefactorName.Core.Entities;

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

        public ActionResult DeleteInvoice()
        {
            Invoice dbItem = InvoiceDomain.Obj.Get(1);

            InvoiceDomain.Obj.Delete(dbItem);

            return View();
        }

        public ActionResult DeleteItem()
        {
            Item dbItem = ItemDomain.Obj.Get(1);

            ItemDomain.Obj.Delete(dbItem);

            return View();
        }

        public ActionResult CreateItem()
        {
            Item item = new Item();

            ItemDomain.Obj.Create(item);

            return View();
        }

        public ActionResult UpdateItem()
        {
            Item dbItem = ItemDomain.Obj.Get(2);
            dbItem.Update("Pen");

            ItemDomain.Obj.Update(dbItem);

            return View();
        }
    }
}
