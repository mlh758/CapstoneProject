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
    public class ExperienceLevelsController : Controller
    {
        private OnCallContext db = new OnCallContext();

        // GET: ExperienceLevels
        public ActionResult Index()
        {
            return View(db.experienceLevel.ToList());
        }

        // GET: ExperienceLevels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExperienceLevel experienceLevel = db.experienceLevel.Find(id);
            if (experienceLevel == null)
            {
                return HttpNotFound();
            }
            return View(experienceLevel);
        }

        // GET: ExperienceLevels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ExperienceLevels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,levelName")] ExperienceLevel experienceLevel)
        {
            if (ModelState.IsValid)
            {
                db.experienceLevel.Add(experienceLevel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(experienceLevel);
        }

        // GET: ExperienceLevels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExperienceLevel experienceLevel = db.experienceLevel.Find(id);
            if (experienceLevel == null)
            {
                return HttpNotFound();
            }
            return View(experienceLevel);
        }

        // POST: ExperienceLevels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,levelName")] ExperienceLevel experienceLevel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(experienceLevel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(experienceLevel);
        }

        // GET: ExperienceLevels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExperienceLevel experienceLevel = db.experienceLevel.Find(id);
            if (experienceLevel == null)
            {
                return HttpNotFound();
            }
            return View(experienceLevel);
        }

        // POST: ExperienceLevels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExperienceLevel experienceLevel = db.experienceLevel.Find(id);
            db.experienceLevel.Remove(experienceLevel);
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
