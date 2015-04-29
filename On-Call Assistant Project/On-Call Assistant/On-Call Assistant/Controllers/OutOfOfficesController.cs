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
using On_Call_Assistant.Group_Code;

namespace On_Call_Assistant.Controllers
{
    public class OutOfOfficesController : Controller
    {
        private OnCallContext db = new OnCallContext();

        // GET: OutOfOffices
        public async Task<ActionResult> Index()
        {
            var outOfOffice = db.outOfOffice.Include(o => o.employeeOut).Include(o => o.reason);
            return View(await outOfOffice.ToListAsync());
        }

        // GET: OutOfOffices/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutOfOffice outOfOffice = await db.outOfOffice.FindAsync(id);
            if (outOfOffice == null)
            {
                return HttpNotFound();
            }
            return View(outOfOffice);
        }

        // GET: OutOfOffices/Create
        public ActionResult Create()
        {
            ViewBag.Employee = new SelectList(db.employees.OrderBy(x => x.lastName), "ID", "employeeName");
            ViewBag.outOfOfficeReasonID = new SelectList(db.outOfOfficeReasons, "ID", "reason");
            return View();
        }

        // POST: OutOfOffices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,numHours,startDate,outOfOfficeReasonID,Employee")] OutOfOffice outOfOffice)
        {
            if (ModelState.IsValid)
            {
                db.outOfOffice.Add(outOfOffice);
                await db.SaveChangesAsync();
                Scheduler scheduler = new Scheduler();
                scheduler.alterOnEmployeeAbsence(outOfOffice.ID);
                return RedirectToAction("Index");
            }

            ViewBag.Employee = new SelectList(db.employees.OrderBy(x => x.lastName), "ID", "employeeName", outOfOffice.Employee);
            ViewBag.outOfOfficeReasonID = new SelectList(db.outOfOfficeReasons, "ID", "reason", outOfOffice.outOfOfficeReasonID);
            return View(outOfOffice);
        }

        // GET: OutOfOffices/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutOfOffice outOfOffice = await db.outOfOffice.FindAsync(id);
            if (outOfOffice == null)
            {
                return HttpNotFound();
            }
            ViewBag.Employee = new SelectList(db.employees.OrderBy(x => x.lastName), "ID", "employeeName", outOfOffice.Employee);
            ViewBag.outOfOfficeReasonID = new SelectList(db.outOfOfficeReasons, "ID", "reason", outOfOffice.outOfOfficeReasonID);
            return View(outOfOffice);
        }

        // POST: OutOfOffices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,numHours,startDate,outOfOfficeReasonID,Employee")] OutOfOffice outOfOffice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(outOfOffice).State = EntityState.Modified;
                await db.SaveChangesAsync();
                Scheduler scheduler = new Scheduler();
                scheduler.alterOnEmployeeAbsence(outOfOffice.ID);
                return RedirectToAction("Index");
            }
            ViewBag.Employee = new SelectList(db.employees.OrderBy(x => x.lastName), "ID", "employeeName", outOfOffice.Employee);
            ViewBag.outOfOfficeReasonID = new SelectList(db.outOfOfficeReasons, "ID", "reason", outOfOffice.outOfOfficeReasonID);
            return View(outOfOffice);
        }

        // GET: OutOfOffices/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutOfOffice outOfOffice = await db.outOfOffice.FindAsync(id);
            if (outOfOffice == null)
            {
                return HttpNotFound();
            }
            return View(outOfOffice);
        }

        // POST: OutOfOffices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            OutOfOffice outOfOffice = await db.outOfOffice.FindAsync(id);
            db.outOfOffice.Remove(outOfOffice);
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
