using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using MVCGrid.Models;
using static MVCGrid.Models.DataContext;
using MVCGrid.Hubs;
using Newtonsoft.Json;

namespace MVCGrid.Controllers
{

    public class HomeController : Controller
    {
       
        DataContext _DB = new DataContext();
        IRepository<WorkersSmall> _RepDB;
        [HttpGet]
        public ActionResult AutocompleteSearch(string term)
        {
            List<string> a = new List<string>();
         
          
            var models = _DB.WorkersSmall.Where(k => k.FirstName.Contains(term)).Select(k => new { value = k.FirstName }).Distinct();
          
            return Json(models, JsonRequestBehavior.AllowGet);
        }
        public HomeController(IRepository<WorkersSmall> repository)
        {
            _RepDB = repository ;
        }
        public HomeController()
        {
            _RepDB = new Repository();
        }
        public ActionResult EditWorker(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            //   Worker worker = (Worker)_DB.WorkersSmall.Find(id);
            Worker worker = (Worker)_RepDB.GetWorker((int)id);
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
        public ActionResult EditWorker([Bind(Include ="Id, FirstName, LastName, Position, Salary")] WorkersSmall worker)
        {

            if (worker == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (ModelState.IsValidField("FirstName") && ModelState.IsValidField("LastName") && ModelState.IsValidField("Position") && ModelState.IsValidField("Salary"))
                {
                    var workerToSave = _RepDB.GetWorker(worker.Id);
                    workerToSave.FirstName = worker.FirstName;
                    workerToSave.LastName = worker.LastName;
                    workerToSave.Position = worker.Position;
                    workerToSave.Salary = worker.Salary;
                    _RepDB.Update(workerToSave);
                    _RepDB.Save();
                    // _DB.Entry(workerToSave).State = System.Data.Entity.EntityState.Modified;
                   // _DB.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    //  Worker worker1 = (Worker)_DB.WorkersSmall.Find(worker.Id);
                     Worker worker1 = (Worker)_RepDB.GetWorker(worker.Id);
                    return View(worker1);
                }
            }
                
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {

            //  Worker worker = (Worker)_DB.WorkersSmall.Find(id);            
             Worker worker = (Worker)_RepDB.GetWorker(id);           
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
            //WorkersSmall worker = _DB.WorkersSmall.Find(id);
            WorkersSmall worker = _RepDB.GetWorker(id);


            if (worker == null)
            {
                return HttpNotFound();
            }
            else
            {
                _RepDB.Delete(id);
                _RepDB.Save();
                return RedirectToAction("Index");
            }
            
        }



        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WorkersSmall worker)
        {
            if (worker == null)
            {
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {

                  //  _DB.Entry(worker).State = System.Data.Entity.EntityState.Added;
                    _RepDB.Create(worker);
                    _RepDB.Save();
                //    _DB.SaveChanges();
                    SendMessage("Добавлен новый работник");
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Create");
                }
            }
           
        }
        [HttpPost]
        public JsonResult JSONWorkerSearch(string firstName)
        {
            var jsondata = _RepDB.GetWorkers().Where(a => a.FirstName==firstName).ToList();
           
            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }
        private void SendMessage(string message)
        {
            // Получаем контекст хаба
            var context =
                Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            // отправляем сообщение
         
            context.Clients.All.displayMessage(message);
        }
        public ActionResult ShowAlcoholics()
        {
            var alcoholics = _RepDB.GetWorkers().Where(a => a.IsAlcoholic).ToList();
            List<Worker> alcoholicsList = new List<Worker>();
            foreach (var item in alcoholics)
            {
                alcoholicsList.Add((Worker)item);
            }
            if (alcoholicsList.Count <= 0)
            {
                return HttpNotFound();
            }
            return PartialView(alcoholicsList);
        }
        public string GetData()
        {
            // var workers = _DB.WorkersSmall.OrderBy(k => k.Id).ToList();
            var workers = _RepDB.GetWorkers().OrderBy(k => k.Id).ToList();
            return JsonConvert.SerializeObject(workers);
        }

        protected override void Dispose(bool disposing)
        {
            _RepDB.Dispose();
            base.Dispose(disposing);
        }
        public ActionResult Index()
        {

            // int pageSize = 10; // количество объектов на страницу          
            //  IEnumerable<WorkersSmall> workersForPage = _DB.WorkersSmall.OrderBy(k=>k.Id).Skip((page - 1) * pageSize).Take(pageSize);
            //  PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems =_DB.WorkersSmall.Count() };
            // IndexViewModel ivm = new IndexViewModel { PageInfo = pageInfo, Workers = workersForPage };

            //  return View(ivm);
            
            return View("Index");

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