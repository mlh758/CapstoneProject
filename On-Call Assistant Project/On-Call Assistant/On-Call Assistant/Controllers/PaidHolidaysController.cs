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
    public class PaidHolidaysController : Controller
    {
        private OnCallContext db = new OnCallContext();

        // GET: PaidHolidays
        public async Task<ActionResult> Index()
        {
            return View(await db.paidHolidays.ToListAsync());
        }

        // GET: PaidHolidays/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaidHoliday paidHoliday = await db.paidHolidays.FindAsync(id);
            if (paidHoliday == null)
            {
                return HttpNotFound();
            }
            return View(paidHoliday);
        }

        // GET: PaidHolidays/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaidHolidays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "paidHolidayID,holidayName,holidayDate")] PaidHoliday paidHoliday)
        {
            if (ModelState.IsValid)
            {
                db.paidHolidays.Add(paidHoliday);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(paidHoliday);
        }

        // GET: PaidHolidays/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaidHoliday paidHoliday = await db.paidHolidays.FindAsync(id);
            if (paidHoliday == null)
            {
                return HttpNotFound();
            }
            return View(paidHoliday);
        }

        // POST: PaidHolidays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "paidHolidayID,holidayName,holidayDate")] PaidHoliday paidHoliday)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paidHoliday).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(paidHoliday);
        }

        // GET: PaidHolidays/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaidHoliday paidHoliday = await db.paidHolidays.FindAsync(id);
            if (paidHoliday == null)
            {
                return HttpNotFound();
            }
            return View(paidHoliday);
        }

        // POST: PaidHolidays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PaidHoliday paidHoliday = await db.paidHolidays.FindAsync(id);
            db.paidHolidays.Remove(paidHoliday);
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
