using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCGrid.Models;



namespace MVCGrid.Controllers
{
   
    public class HomeController : Controller
    {
       IEnumerable<Workers> _Workers;
        DataContext _DataContext = new DataContext();

       
        public ActionResult Index()
        {
            _Workers = _DataContext.Workers.Take(5000);
            ViewBag.Workers = _Workers;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}