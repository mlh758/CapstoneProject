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
    public class OutOfOfficeReasonsController : Controller
    {
        private OnCallContext db = new OnCallContext();

        // GET: OutOfOfficeReasons
        public ActionResult Index()
        {
            return View(db.outOfOfficeReasons.ToList());
        }

        // GET: OutOfOfficeReasons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutOfOfficeReason outOfOfficeReason = db.outOfOfficeReasons.Find(id);
            if (outOfOfficeReason == null)
            {
                return HttpNotFound();
            }
            return View(outOfOfficeReason);
        }

        // GET: OutOfOfficeReasons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OutOfOfficeReasons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,reason")] OutOfOfficeReason outOfOfficeReason)
        {
            if (ModelState.IsValid)
            {
                db.outOfOfficeReasons.Add(outOfOfficeReason);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(outOfOfficeReason);
        }

        // GET: OutOfOfficeReasons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutOfOfficeReason outOfOfficeReason = db.outOfOfficeReasons.Find(id);
            if (outOfOfficeReason == null)
            {
                return HttpNotFound();
            }
            return View(outOfOfficeReason);
        }

        // POST: OutOfOfficeReasons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,reason")] OutOfOfficeReason outOfOfficeReason)
        {
            if (ModelState.IsValid)
            {
                db.Entry(outOfOfficeReason).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(outOfOfficeReason);
        }

        // GET: OutOfOfficeReasons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutOfOfficeReason outOfOfficeReason = db.outOfOfficeReasons.Find(id);
            if (outOfOfficeReason == null)
            {
                return HttpNotFound();
            }
            return View(outOfOfficeReason);
        }

        // POST: OutOfOfficeReasons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OutOfOfficeReason outOfOfficeReason = db.outOfOfficeReasons.Find(id);
            db.outOfOfficeReasons.Remove(outOfOfficeReason);
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
