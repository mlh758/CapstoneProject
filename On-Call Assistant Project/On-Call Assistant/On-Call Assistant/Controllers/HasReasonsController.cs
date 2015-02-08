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
    public class HasReasonsController : Controller
    {
        private OnCallContext db = new OnCallContext();

        // GET: HasReasons
        public ActionResult Index()
        {
            var hasReason = db.hasReason.Include(h => h.outOfOffice).Include(h => h.outOfOfficeReason);
            return View(hasReason.ToList());
        }

        // GET: HasReasons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HasReason hasReason = db.hasReason.Find(id);
            if (hasReason == null)
            {
                return HttpNotFound();
            }
            return View(hasReason);
        }

        // GET: HasReasons/Create
        public ActionResult Create()
        {
            ViewBag.outOfOfficeID = new SelectList(db.outOfOffice, "ID", "_date");
            ViewBag.outOfOfficeReasonID = new SelectList(db.outOfOfficeReasons, "ID", "reason");
            return View();
        }

        // POST: HasReasons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,outOfOfficeID,outOfOfficeReasonID")] HasReason hasReason)
        {
            if (ModelState.IsValid)
            {
                db.hasReason.Add(hasReason);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.outOfOfficeID = new SelectList(db.outOfOffice, "ID", "_date", hasReason.outOfOfficeID);
            ViewBag.outOfOfficeReasonID = new SelectList(db.outOfOfficeReasons, "ID", "reason", hasReason.outOfOfficeReasonID);
            return View(hasReason);
        }

        // GET: HasReasons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HasReason hasReason = db.hasReason.Find(id);
            if (hasReason == null)
            {
                return HttpNotFound();
            }
            ViewBag.outOfOfficeID = new SelectList(db.outOfOffice, "ID", "_date", hasReason.outOfOfficeID);
            ViewBag.outOfOfficeReasonID = new SelectList(db.outOfOfficeReasons, "ID", "reason", hasReason.outOfOfficeReasonID);
            return View(hasReason);
        }

        // POST: HasReasons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,outOfOfficeID,outOfOfficeReasonID")] HasReason hasReason)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hasReason).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.outOfOfficeID = new SelectList(db.outOfOffice, "ID", "_date", hasReason.outOfOfficeID);
            ViewBag.outOfOfficeReasonID = new SelectList(db.outOfOfficeReasons, "ID", "reason", hasReason.outOfOfficeReasonID);
            return View(hasReason);
        }

        // GET: HasReasons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HasReason hasReason = db.hasReason.Find(id);
            if (hasReason == null)
            {
                return HttpNotFound();
            }
            return View(hasReason);
        }

        // POST: HasReasons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HasReason hasReason = db.hasReason.Find(id);
            db.hasReason.Remove(hasReason);
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
