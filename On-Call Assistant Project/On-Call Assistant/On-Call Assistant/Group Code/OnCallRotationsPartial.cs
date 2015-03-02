using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using On_Call_Assistant.DAL;
using On_Call_Assistant.Models;
using On_Call_Assistant.Group_Code;

namespace On_Call_Assistant.Controllers
{
    public partial class OnCallRotationsController : Controller
    {
        private string path = AppDomain.CurrentDomain.BaseDirectory + "\\App_Data\\EmpSch.csv";
        
        public ActionResult generateSchedule()
        {
            DateTime start, end, last;
            start = DateTime.Today;
            last = LinqQueries.LastRotation(db);
            if (last > start)
            {
                start = last;
            }
            end = start.AddDays(40);
            List<OnCallRotation> schedule = Behavior.generateSchedule(db, LinqQueries.GetEmployees(db), start, end);
            LinqQueries.SaveRotations(db, schedule);
            return View(db.onCallRotations.ToList());
        }

        public ActionResult DownloadSchedule()
        {
            Behavior.CreateCSVFile(db.onCallRotations.ToList(), path);
            return File(path, "text/plain", "EmployeeSchedule.csv");
        }
    }
}