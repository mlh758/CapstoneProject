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
    public class OutOfOfficeReasonsController : Controller
    {
        private OnCallContext db = new OnCallContext();

        // GET: OutOfOfficeReasons
        public async Task<ActionResult> Index()
        {
            return View(await db.outOfOfficeReasons.ToListAsync());
        }

        // GET: OutOfOfficeReasons/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutOfOfficeReason outOfOfficeReason = await db.outOfOfficeReasons.FindAsync(id);
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
        public async Task<ActionResult> Create([Bind(Include = "ID,reason,reasonDisplayColor")] OutOfOfficeReason outOfOfficeReason)
        {
            if (ModelState.IsValid)
            {
                db.outOfOfficeReasons.Add(outOfOfficeReason);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(outOfOfficeReason);
        }

        // GET: OutOfOfficeReasons/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutOfOfficeReason outOfOfficeReason = await db.outOfOfficeReasons.FindAsync(id);
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
        public async Task<ActionResult> Edit([Bind(Include = "ID,reason,reasonDisplayColor")] OutOfOfficeReason outOfOfficeReason)
        {
            if (ModelState.IsValid)
            {
                db.Entry(outOfOfficeReason).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(outOfOfficeReason);
        }

        // GET: OutOfOfficeReasons/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutOfOfficeReason outOfOfficeReason = await db.outOfOfficeReasons.FindAsync(id);
            if (outOfOfficeReason == null)
            {
                return HttpNotFound();
            }
            return View(outOfOfficeReason);
        }

        // POST: OutOfOfficeReasons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            OutOfOfficeReason outOfOfficeReason = await db.outOfOfficeReasons.FindAsync(id);
            db.outOfOfficeReasons.Remove(outOfOfficeReason);
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
