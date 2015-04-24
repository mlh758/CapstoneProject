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
    public partial class OnCallRotationsController : Controller
    {
        private OnCallContext db = new OnCallContext();

        // GET: OnCallRotations
        /*public async Task<ActionResult> Index()
        {
            var onCallRotations = db.onCallRotations.Include(o => o.employee);
            return View(await onCallRotations.ToListAsync());
        }*/

        // GET: OnCallRotations/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OnCallRotation onCallRotation = await db.onCallRotations.FindAsync(id);
            if (onCallRotation == null)
            {
                return HttpNotFound();
            }
            return View(onCallRotation);
        }

        // GET: OnCallRotations/Create
        public ActionResult Create()
        {
            ViewBag.employeeID = new SelectList(db.employees.OrderBy(x => x.lastName), "ID", "employeeName");
            return View();
        }

        // POST: OnCallRotations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "rotationID,startDate,endDate,isPrimary,employeeID")] OnCallRotation onCallRotation)
        {
            if (ModelState.IsValid)
            {
                db.onCallRotations.Add(onCallRotation);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.employeeID = new SelectList(db.employees.OrderBy(x => x.lastName), "ID", "employeeName", onCallRotation.employeeID);
            return View(onCallRotation);
        }

        // GET: OnCallRotations/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OnCallRotation onCallRotation = await db.onCallRotations.FindAsync(id);
            if (onCallRotation == null)
            {
                return HttpNotFound();
            }
            ViewBag.employeeID = new SelectList(db.employees.OrderBy(x => x.lastName), "ID", "employeeName", onCallRotation.employeeID);
            return View(onCallRotation);
        }

        // POST: OnCallRotations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "rotationID,startDate,endDate,isPrimary,employeeID")] OnCallRotation onCallRotation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(onCallRotation).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.employeeID = new SelectList(db.employees.OrderBy(x => x.lastName), "ID", "employeeName", onCallRotation.employeeID);
            return View(onCallRotation);
        }

        // GET: OnCallRotations/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OnCallRotation onCallRotation = await db.onCallRotations.FindAsync(id);
            if (onCallRotation == null)
            {
                return HttpNotFound();
            }
            return View(onCallRotation);
        }

        // POST: OnCallRotations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            OnCallRotation onCallRotation = await db.onCallRotations.FindAsync(id);
            db.onCallRotations.Remove(onCallRotation);
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
