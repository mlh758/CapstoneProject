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
    public class IsOutOfOfficesController : Controller
    {
        private OnCallContext db = new OnCallContext();

        // GET: IsOutOfOffices
        public ActionResult Index()
        {
            var isOutOfOffice = db.isOutOfOffice.Include(i => i.employee).Include(i => i.outOfOffice);
            return View(isOutOfOffice.ToList());
        }

        // GET: IsOutOfOffices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IsOutOfOffice isOutOfOffice = db.isOutOfOffice.Find(id);
            if (isOutOfOffice == null)
            {
                return HttpNotFound();
            }
            return View(isOutOfOffice);
        }

        // GET: IsOutOfOffices/Create
        public ActionResult Create()
        {
            ViewBag.employeeID = new SelectList(db.employees, "ID", "firstName");
            ViewBag.outOfOfficeID = new SelectList(db.outOfOffice, "ID", "_date");
            return View();
        }

        // POST: IsOutOfOffices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,employeeID,outOfOfficeID")] IsOutOfOffice isOutOfOffice)
        {
            if (ModelState.IsValid)
            {
                db.isOutOfOffice.Add(isOutOfOffice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.employeeID = new SelectList(db.employees, "ID", "firstName", isOutOfOffice.employeeID);
            ViewBag.outOfOfficeID = new SelectList(db.outOfOffice, "ID", "_date", isOutOfOffice.outOfOfficeID);
            return View(isOutOfOffice);
        }

        // GET: IsOutOfOffices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IsOutOfOffice isOutOfOffice = db.isOutOfOffice.Find(id);
            if (isOutOfOffice == null)
            {
                return HttpNotFound();
            }
            ViewBag.employeeID = new SelectList(db.employees, "ID", "firstName", isOutOfOffice.employeeID);
            ViewBag.outOfOfficeID = new SelectList(db.outOfOffice, "ID", "_date", isOutOfOffice.outOfOfficeID);
            return View(isOutOfOffice);
        }

        // POST: IsOutOfOffices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,employeeID,outOfOfficeID")] IsOutOfOffice isOutOfOffice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(isOutOfOffice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.employeeID = new SelectList(db.employees, "ID", "firstName", isOutOfOffice.employeeID);
            ViewBag.outOfOfficeID = new SelectList(db.outOfOffice, "ID", "_date", isOutOfOffice.outOfOfficeID);
            return View(isOutOfOffice);
        }

        // GET: IsOutOfOffices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IsOutOfOffice isOutOfOffice = db.isOutOfOffice.Find(id);
            if (isOutOfOffice == null)
            {
                return HttpNotFound();
            }
            return View(isOutOfOffice);
        }

        // POST: IsOutOfOffices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IsOutOfOffice isOutOfOffice = db.isOutOfOffice.Find(id);
            db.isOutOfOffice.Remove(isOutOfOffice);
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
