using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult> Index()
        {
            var hasHolidays = db.hasHolidays.Include(h => h.rotation);
            return View(await hasHolidays.ToListAsync());
        }

        // GET: HasPaidHolidays/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HasPaidHoliday hasPaidHoliday = await db.hasHolidays.FindAsync(id);
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
        public async Task<ActionResult> Create([Bind(Include = "onCallRotationID,paidHolidayID")] HasPaidHoliday hasPaidHoliday)
        {
            if (ModelState.IsValid)
            {
                db.hasHolidays.Add(hasPaidHoliday);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.onCallRotationID = new SelectList(db.onCallRotations, "ID", "ID", hasPaidHoliday.onCallRotationID);
            return View(hasPaidHoliday);
        }

        // GET: HasPaidHolidays/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HasPaidHoliday hasPaidHoliday = await db.hasHolidays.FindAsync(id);
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
        public async Task<ActionResult> Edit([Bind(Include = "onCallRotationID,paidHolidayID")] HasPaidHoliday hasPaidHoliday)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hasPaidHoliday).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.onCallRotationID = new SelectList(db.onCallRotations, "ID", "ID", hasPaidHoliday.onCallRotationID);
            return View(hasPaidHoliday);
        }

        // GET: HasPaidHolidays/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HasPaidHoliday hasPaidHoliday = await db.hasHolidays.FindAsync(id);
            if (hasPaidHoliday == null)
            {
                return HttpNotFound();
            }
            return View(hasPaidHoliday);
        }

        // POST: HasPaidHolidays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            HasPaidHoliday hasPaidHoliday = await db.hasHolidays.FindAsync(id);
            db.hasHolidays.Remove(hasPaidHoliday);
            await db.SaveChangesAsync();
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
