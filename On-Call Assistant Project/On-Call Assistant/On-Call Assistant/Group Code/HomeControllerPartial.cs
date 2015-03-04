using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using On_Call_Assistant.DAL;
using On_Call_Assistant.Models;


namespace On_Call_Assistant.Controllers
{
    public partial class HomeController : Controller
    {
        private OnCallContext db = new OnCallContext();
        public ActionResult RotationData()
        {
            IList<CalendarObject> rotationList = new List<CalendarObject>();
            var onCallRotations = db.onCallRotations.Include(o => o.employee);
            foreach (var rotation in onCallRotations)
            {
                rotationList.Add(new CalendarObject
                {
                    id = rotation.employee.Application,
                    title = rotation.employee.firstName + " " + rotation.employee.lastName,
                    start = rotation.startDate.ToString("u"),
                    end = rotation.endDate.ToString("u")
                });
            }

            return Json(rotationList, JsonRequestBehavior.AllowGet);
        }

    }

    public class CalendarObject
    {
        public int id { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }

    }
}