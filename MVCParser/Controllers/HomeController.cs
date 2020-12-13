using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MVCParser.Models;
using Newtonsoft.Json;

namespace MVCParser.Controllers
{
    public class HomeController : Controller
    {


        DBModel Db = new DBModel();
        List<Column> Columns = new List<Column>();

        public ActionResult Index()
        {
           
          
            return View("Index");

        }
        

        [HttpPost]
        public ActionResult GetData(List<string> myKey)
        {

            var jsonResult = Db.Workers.OrderBy(k => k.Id).Take(100).Select(k => k).ToList();
            foreach (var item in jsonResult)
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(item);
                foreach (PropertyDescriptor property in properties)
                {
                    if (!myKey.Contains(property.Name))
                    {
                        property.SetValue(item, null);

                    }
                    
                }

            }
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllColumns()
        {
            GetColumnsInfo();

           
            return Json(Columns, JsonRequestBehavior.AllowGet);

        }
        private void GetColumnsInfo()
        {
          
            var data = Db.Workers.FirstOrDefault();
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(data);
            foreach (PropertyDescriptor property in properties)
            {
                bool isExist = false;
                for (int i = 0; i < Columns.Count; i++)
                {
                    if (Columns[i].Name == property.Name)
                    {
                        isExist = true;
                        break;
                    }
                }
                if (!isExist)
                {
                    Column col = new Column();
                    col.Name = property.Name;
                    col.Type = property.PropertyType.ToString();
                    Columns.Add(col);
                }
            }
        }


    }
    public class Column
    {
        public string Name { get; set; }
        public string Type { get; set; }
      

    }
}

