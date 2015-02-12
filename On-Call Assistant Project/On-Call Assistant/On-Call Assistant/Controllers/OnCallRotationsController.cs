using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using On_Call_Assistant.DAL;
using On_Call_Assistant.Models;
using On_Call_Assistant.Group_Code;
using System.Text;
using System.IO;

namespace On_Call_Assistant.Controllers
{
    public class OnCallRotationsController : Controller
    {
        private OnCallContext db = new OnCallContext();
        private string path = AppDomain.CurrentDomain.BaseDirectory + "\\App_Data\\EmpSch.csv";
        // 
        public ActionResult generateSchedule()
        {
            //System.Diagnostics.Debug.WriteLine("SomeText");
            DateTime start, end;
            start = DateTime.Today;
            end = start.AddDays(40);
            Behavior bh = new Behavior();
            List<OnCallRotation> schedule = bh.generateSchedule(LinqQueries.GetEmployees(db), start, end);
            CreateCSVFile(db.onCallRotations.ToList());
            LinqQueries.SaveRotations(db, schedule);
            return View(db.onCallRotations.ToList());
        }

        
       

        // GET: OnCallRotations
        public ActionResult Index()
        {
            return View(db.onCallRotations.ToList());
        }

        public void CreateCSVFile(List<OnCallRotation> list)
        {
            string delimter = ",";
            int length = list.Count;

            using (TextWriter writer = new StreamWriter(path))
            {
                writer.WriteLine("Start Date, End Date, Employee ID");//should not be hard coded
                foreach (var item in list)
                {
                    writer.WriteLine(string.Join(delimter, item.startDate, item.endDate, item.employeeID));
                }
            }
            
        }
    
        public ActionResult DownloadSchedule()
        {
            return File(path, "text/plain", "EmployeeSchedule.csv");   
        }
        

        // GET: OnCallRotations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OnCallRotation onCallRotation = db.onCallRotations.Find(id);
            if (onCallRotation == null)
            {
                return HttpNotFound();
            }
            return View(onCallRotation);
        }

        // GET: OnCallRotations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OnCallRotations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,startDate,endDate,isPrimatry,employeeID")] OnCallRotation onCallRotation)
        {
            if (ModelState.IsValid)
            {
                db.onCallRotations.Add(onCallRotation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(onCallRotation);
        }

        // GET: OnCallRotations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OnCallRotation onCallRotation = db.onCallRotations.Find(id);
            if (onCallRotation == null)
            {
                return HttpNotFound();
            }
            return View(onCallRotation);
        }

        // POST: OnCallRotations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,startDate,endDate,isPrimatry,employeeID")] OnCallRotation onCallRotation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(onCallRotation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(onCallRotation);
        }

        // GET: OnCallRotations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OnCallRotation onCallRotation = db.onCallRotations.Find(id);
            if (onCallRotation == null)
            {
                return HttpNotFound();
            }
            return View(onCallRotation);
        }

        // POST: OnCallRotations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OnCallRotation onCallRotation = db.onCallRotations.Find(id);
            db.onCallRotations.Remove(onCallRotation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
