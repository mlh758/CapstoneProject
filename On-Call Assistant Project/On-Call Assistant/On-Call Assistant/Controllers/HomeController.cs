using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace On_Call_Assistant.Controllers
{
    public partial class HomeController : Controller
    {
        public ActionResult Index()
        {
            On_Call_Assistant.DAL.OnCallContext db = new DAL.OnCallContext();
            ViewBag.apps = new SelectList(db.applications, "ID", "appName");
            ViewBag.emps = new SelectList(db.employees.OrderBy(x => x.lastName), "ID", "employeeName");
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

        public ActionResult Printable()
        {
            return View();
        }
    }
}