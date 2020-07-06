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
        IRepository<WorkersSmall> _MsSqlDB;
        public HomeController(IRepository<WorkersSmall> repository)
        {
            _MsSqlDB = repository;
        }
        public HomeController()
        {
            _MsSqlDB = new Repository();
        }
        public string GetData()
        {
            var workers = _MsSqlDB.GetWorkers().OrderBy(k => k.Id).ToList();
            return JsonConvert.SerializeObject(workers);
        }
        public ActionResult Index()
        {
            return View("Index");
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
                    _MsSqlDB.Create(worker);
                    _MsSqlDB.Save();
                    SendMessage("Добавлен новый работник");
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Create");
                }
            }

        }
        public ActionResult EditWorker(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            WorkerSmall worker = (WorkerSmall)_MsSqlDB.GetWorker((int)id);
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
        public ActionResult EditWorker([Bind(Include = "Id, FirstName, LastName, Position, Salary")] WorkersSmall worker)
        {

            if (worker == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (ModelState.IsValidField("FirstName") && ModelState.IsValidField("LastName") && ModelState.IsValidField("Position") && ModelState.IsValidField("Salary"))
                {
                    var workerToSave = _MsSqlDB.GetWorker(worker.Id);
                    workerToSave.FirstName = worker.FirstName;
                    workerToSave.LastName = worker.LastName;
                    workerToSave.Position = worker.Position;
                    workerToSave.Salary = worker.Salary;
                    _MsSqlDB.Update(workerToSave);
                    _MsSqlDB.Save();
                    return RedirectToAction("Index");
                }
                else
                {
                    WorkerSmall worker1 = (WorkerSmall)_MsSqlDB.GetWorker(worker.Id);
                    return View(worker1);
                }
            }

        }
        [HttpGet]
        public ActionResult Delete(int id)
        {

            WorkerSmall worker = (WorkerSmall)_MsSqlDB.GetWorker(id);
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

            WorkerSmall worker = (WorkerSmall)_MsSqlDB.GetWorker(id);

            if (worker == null)
            {
                return HttpNotFound();
            }
            else
            {
                _MsSqlDB.Delete(id);
                _MsSqlDB.Save();
                return RedirectToAction("Index");
            }

        }
         [HttpGet]
        public ActionResult AutocompleteSearch(string term)
        {
            List<string> a = new List<string>();
         
          
            var models = _DB.WorkersSmall.Where(k => k.FirstName.Contains(term)).Select(k => new { value = k.FirstName }).Distinct();
          
            return Json(models, JsonRequestBehavior.AllowGet);
        }            
        [HttpPost]
        public JsonResult JSONWorkerSearch(string firstName)
        {
            var jsondata = _MsSqlDB.GetWorkers().Where(a => a.FirstName==firstName).ToList();           
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
            var alcoholics = _MsSqlDB.GetWorkers().Where(a => a.IsAlcoholic).ToList();
            List<WorkersSmall> alcoholicsList = new List<WorkersSmall>();
            foreach (var item in alcoholics)
            {
                alcoholicsList.Add(item);
            }
            if (alcoholicsList.Count <= 0)
            {
                return HttpNotFound();
            }
            return PartialView(alcoholicsList);
        }     

        protected override void Dispose(bool disposing)
        {
            _MsSqlDB.Dispose();
            base.Dispose(disposing);
        }     

       
    }
}