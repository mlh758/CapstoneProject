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
    public class IsOnRotationsController : Controller
    {
        private OnCallContext db = new OnCallContext();

        // GET: IsOnRotations1
        public ActionResult Index()
        {
            var isOnRotation = db.isOnRotation.Include(i => i.employee).Include(i => i.rotation);
            return View(isOnRotation.ToList());
        }

        // GET: IsOnRotations1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IsOnRotation isOnRotation = db.isOnRotation.Find(id);
            if (isOnRotation == null)
            {
                return HttpNotFound();
            }
            return View(isOnRotation);
        }

        // GET: IsOnRotations1/Create
        public ActionResult Create()
        {
            ViewBag.employeeID = new SelectList(db.employees, "ID", "firstName");
            ViewBag.rotationID = new SelectList(db.onCallRotations, "ID", "startDate");
            return View();
        }

        // POST: IsOnRotations1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,employeeID,rotationID")] IsOnRotation isOnRotation)
        {
            if (ModelState.IsValid)
            {
                db.isOnRotation.Add(isOnRotation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.employeeID = new SelectList(db.employees, "ID", "firstName", isOnRotation.employeeID);
            ViewBag.rotationID = new SelectList(db.onCallRotations, "ID", "startDate", isOnRotation.rotationID);
            return View(isOnRotation);
        }

        // GET: IsOnRotations1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IsOnRotation isOnRotation = db.isOnRotation.Find(id);
            if (isOnRotation == null)
            {
                return HttpNotFound();
            }
            ViewBag.employeeID = new SelectList(db.employees, "ID", "firstName", isOnRotation.employeeID);
            ViewBag.rotationID = new SelectList(db.onCallRotations, "ID", "startDate", isOnRotation.rotationID);
            return View(isOnRotation);
        }

        // POST: IsOnRotations1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,employeeID,rotationID")] IsOnRotation isOnRotation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(isOnRotation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.employeeID = new SelectList(db.employees, "ID", "firstName", isOnRotation.employeeID);
            ViewBag.rotationID = new SelectList(db.onCallRotations, "ID", "startDate", isOnRotation.rotationID);
            return View(isOnRotation);
        }

        // GET: IsOnRotations1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IsOnRotation isOnRotation = db.isOnRotation.Find(id);
            if (isOnRotation == null)
            {
                return HttpNotFound();
            }
            return View(isOnRotation);
        }

        // POST: IsOnRotations1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IsOnRotation isOnRotation = db.isOnRotation.Find(id);
            db.isOnRotation.Remove(isOnRotation);
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
