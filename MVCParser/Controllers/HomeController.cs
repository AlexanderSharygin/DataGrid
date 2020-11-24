using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCParser.Models;

namespace MVCParser.Controllers
{
    public class HomeController : Controller
    {
        DBModel Db = new DBModel();
        public ActionResult Index()
          {
            return View("Index");
          }
        [HttpPost]
        public ActionResult WorkerSearch()
        {
            var jsonResult = Db.Workers.OrderBy(k=>k.Id).Take(100).Select(k => k).ToList();

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

    }
}