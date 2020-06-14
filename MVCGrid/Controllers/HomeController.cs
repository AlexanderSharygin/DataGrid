using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using MVCGrid.Models;
using static MVCGrid.Models.DataContext;

namespace MVCGrid.Controllers
{

    public class HomeController : Controller
    {

        DataContext _DB = new DataContext();
        [HttpGet]
        public ActionResult EditWorker(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Worker worker = (Worker)_DB.WorkersSmall.Find(id);
            if (worker != null)
            {
                return View(worker);
            }
            else
            {
                return HttpNotFound();
            }

        }
        [HttpPost]
        public ActionResult EditWorker(WorkersSmall worker)
        {

            _DB.Entry(worker).State = System.Data.Entity.EntityState.Modified;
            _DB.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            
                Worker worker = (Worker)_DB.WorkersSmall.Find(id);            
                if (worker == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    return View(worker);
                }
           

        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            WorkersSmall worker = _DB.WorkersSmall.Find(id);

            if (worker == null)
            {
                return HttpNotFound();
            }
            else
            {
                _DB.WorkersSmall.Remove(worker);
                _DB.SaveChanges();
                return RedirectToAction("Index");
            }
            
        }



        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(WorkersSmall worker)
        {
            _DB.WorkersSmall.Add(worker);
            _DB.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Index(int page = 1)
        {
            
           int pageSize = 10; // количество объектов на страницу          
            IEnumerable<WorkersSmall> workersForPage = _DB.WorkersSmall.OrderBy(k=>k.Id).Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems =_DB.WorkersSmall.Count() };
            IndexViewModel ivm = new IndexViewModel { PageInfo = pageInfo, Workers = workersForPage };
            return View(ivm);

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