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
        public async Task<ActionResult> Index(string sortOrder)
        {
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewBag.EmpSortParm = sortOrder == "emp" ? "emp_desc" : "emp";

            var onCallRotations = db.onCallRotations.Include(o => o.employee);

            switch (sortOrder)
            {
                case "date_desc":
                    onCallRotations = onCallRotations.OrderByDescending(r => r.startDate);
                    break;
                case "emp":
                    onCallRotations = onCallRotations.OrderBy(r => r.employee.lastName);
                    break;
                case "emp_desc":
                    onCallRotations = onCallRotations.OrderByDescending(r => r.employee.lastName);
                    break;
                default:
                    onCallRotations = onCallRotations.OrderBy(r => r.startDate);
                    break;
            }

            return View(await onCallRotations.ToListAsync());
        }

        private string path = AppDomain.CurrentDomain.BaseDirectory + "\\App_Data\\EmpSch.csv";
        
        public ActionResult generateSchedule()
        {
            Scheduler generator = new Scheduler(db);
            DateTime start, end, last;
            start = getFutureDay(DateTime.Today, DayOfWeek.Wednesday);
            start = start.AddHours(9); //Start at 9AM
            last = LinqQueries.LastRotation(db);
            if (last > start)
            {
                start = last.AddDays(1);
            }
            end = start.AddMonths(4);
            List<OnCallRotation> schedule = generator.generateSchedule(start, end);
            LinqQueries.SaveRotations(db, schedule);
            return Redirect("/Home/Index");
        }

        public ActionResult regenerateSchedule(string begin, string end)
        {
            Scheduler generator = new Scheduler(db);

            //Don't run if either parameter is empty
            if (begin == "" || end == "")
                return Redirect("/Home/Index");

            try
            {
                DateTime start = DateTime.Parse(begin);
                //Ensure start happens on Wednesday to avoid deleting the wrong rotations
                start = getFutureDay(start, DayOfWeek.Wednesday);
                DateTime finish = DateTime.Parse(end);
                if (finish <= start)
                    return Redirect("/Home/Index");

                List<OnCallRotation> schedule = generator.regenerateSchedule(start, finish);
                LinqQueries.SaveRotations(db, schedule);
            }
            catch (FormatException)
            {

                return Redirect("/Home/Index");
            }


            return Redirect("/Home/Index");
        }

        public ActionResult DownloadSchedule()
        {
            Scheduler.CreateCSVFile(db.onCallRotations.ToList(), path);
            return File(path, "text/plain", "EmployeeSchedule.csv");
        }
        private DateTime getFutureDay(DateTime start, DayOfWeek day)
        {
            int daysToAdd = (day - start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }
    }
}