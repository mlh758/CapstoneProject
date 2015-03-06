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

namespace On_Call_Assistant.Controllers
{
    public class HasPaidHolidaysController : Controller
    {
        private OnCallContext db = new OnCallContext();

        // GET: HasPaidHolidays
        public ActionResult Index()
        {
            var hasHolidays = db.hasHolidays.Include(h => h.rotation);
            return View(hasHolidays.ToList());
        }

        // GET: HasPaidHolidays/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HasPaidHoliday hasPaidHoliday = db.hasHolidays.Find(id);
            if (hasPaidHoliday == null)
            {
                return HttpNotFound();
            }
            return View(hasPaidHoliday);
        }

        // GET: HasPaidHolidays/Create
        public ActionResult Create()
        {
            ViewBag.onCallRotationID = new SelectList(db.onCallRotations, "ID", "ID");
            return View();
        }

        // POST: HasPaidHolidays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,paidHolidayID,onCallRotationID")] HasPaidHoliday hasPaidHoliday)
        {
            if (ModelState.IsValid)
            {
                db.hasHolidays.Add(hasPaidHoliday);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.onCallRotationID = new SelectList(db.onCallRotations, "ID", "ID", hasPaidHoliday.onCallRotationID);
            return View(hasPaidHoliday);
        }

        // GET: HasPaidHolidays/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HasPaidHoliday hasPaidHoliday = db.hasHolidays.Find(id);
            if (hasPaidHoliday == null)
            {
                return HttpNotFound();
            }
            ViewBag.onCallRotationID = new SelectList(db.onCallRotations, "ID", "ID", hasPaidHoliday.onCallRotationID);
            return View(hasPaidHoliday);
        }

        // POST: HasPaidHolidays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,paidHolidayID,onCallRotationID")] HasPaidHoliday hasPaidHoliday)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hasPaidHoliday).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.onCallRotationID = new SelectList(db.onCallRotations, "ID", "ID", hasPaidHoliday.onCallRotationID);
            return View(hasPaidHoliday);
        }

        // GET: HasPaidHolidays/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HasPaidHoliday hasPaidHoliday = db.hasHolidays.Find(id);
            if (hasPaidHoliday == null)
            {
                return HttpNotFound();
            }
            return View(hasPaidHoliday);
        }

        // POST: HasPaidHolidays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HasPaidHoliday hasPaidHoliday = db.hasHolidays.Find(id);
            db.hasHolidays.Remove(hasPaidHoliday);
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
