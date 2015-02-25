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
    public class OutOfOfficesController : Controller
    {
        private OnCallContext db = new OnCallContext();

        // GET: OutOfOffices
        public ActionResult Index()
        {
            var outOfOffice = db.outOfOffice.Include(o => o.reason);
            return View(outOfOffice.ToList());
        }

        // GET: OutOfOffices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutOfOffice outOfOffice = db.outOfOffice.Find(id);
            if (outOfOffice == null)
            {
                return HttpNotFound();
            }
            return View(outOfOffice);
        }

        // GET: OutOfOffices/Create
        public ActionResult Create()
        {
            ViewBag.outOfOfficeReasonID = new SelectList(db.outOfOfficeReasons, "ID", "reason");
            return View();
        }

        // POST: OutOfOffices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,numHours,startDate,outOfOfficeReasonID")] OutOfOffice outOfOffice)
        {
            if (ModelState.IsValid)
            {
                db.outOfOffice.Add(outOfOffice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.outOfOfficeReasonID = new SelectList(db.outOfOfficeReasons, "ID", "reason", outOfOffice.outOfOfficeReasonID);
            return View(outOfOffice);
        }

        // GET: OutOfOffices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutOfOffice outOfOffice = db.outOfOffice.Find(id);
            if (outOfOffice == null)
            {
                return HttpNotFound();
            }
            ViewBag.outOfOfficeReasonID = new SelectList(db.outOfOfficeReasons, "ID", "reason", outOfOffice.outOfOfficeReasonID);
            return View(outOfOffice);
        }

        // POST: OutOfOffices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,numHours,startDate,outOfOfficeReasonID")] OutOfOffice outOfOffice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(outOfOffice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.outOfOfficeReasonID = new SelectList(db.outOfOfficeReasons, "ID", "reason", outOfOffice.outOfOfficeReasonID);
            return View(outOfOffice);
        }

        // GET: OutOfOffices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutOfOffice outOfOffice = db.outOfOffice.Find(id);
            if (outOfOffice == null)
            {
                return HttpNotFound();
            }
            return View(outOfOffice);
        }

        // POST: OutOfOffices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OutOfOffice outOfOffice = db.outOfOffice.Find(id);
            db.outOfOffice.Remove(outOfOffice);
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
