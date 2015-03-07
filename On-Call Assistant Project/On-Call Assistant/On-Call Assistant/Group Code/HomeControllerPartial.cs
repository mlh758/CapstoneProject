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
            System.Collections.Hashtable applicationColors = getApplicationColors();
            foreach (var rotation in onCallRotations)
            {
                rotationList.Add(new CalendarObject
                {
                    id = rotation.employee.Application,
                    title = rotation.employee.firstName + " " + rotation.employee.lastName,
                    start = rotation.startDate.ToString("u"),
                    end = rotation.endDate.AddDays(1).ToString("u"),
                    color = applicationColors[rotation.employee.Application].ToString(),
                    url = String.Format("OnCallRotations/Details/{0}",rotation.ID)
                });
            }

            return Json(rotationList, JsonRequestBehavior.AllowGet);
        }
        private System.Collections.Hashtable getApplicationColors()
        {
            System.Collections.Hashtable colors = new System.Collections.Hashtable();
            colors.Add(3, "Cyan");
            colors.Add(5, "DarkSeaGreen");
            return colors;
        }

    }

    public class CalendarObject
    {
        public int id { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string color { get; set; }
        public string url { get; set; }
        public string allDay { get { return "true"; } }

    }
    
}