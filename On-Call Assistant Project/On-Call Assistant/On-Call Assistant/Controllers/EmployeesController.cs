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
    public partial class EmployeesController : Controller
    {
        private OnCallContext db = new OnCallContext();

        // GET: Employees
        /*public async Task<ActionResult> Index()
        {
            var employees = db.employees.Include(e => e.assignedApplication).Include(e => e.experienceLevel);
            return View(await employees.ToListAsync());
        }*/

        // GET: Employees/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = await db.employees.FindAsync(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.Application = new SelectList(db.applications, "ID", "appName");
            ViewBag.Experience = new SelectList(db.experienceLevel, "ID", "levelName");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,firstName,lastName,alottedVacationHours,email,hiredDate,birthday,Application,Experience")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.employees.Add(employee);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Application = new SelectList(db.applications, "ID", "appName", employee.Application);
            ViewBag.Experience = new SelectList(db.experienceLevel, "ID", "levelName", employee.Experience);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = await db.employees.FindAsync(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.Application = new SelectList(db.applications, "ID", "appName", employee.Application);
            ViewBag.Experience = new SelectList(db.experienceLevel, "ID", "levelName", employee.Experience);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,firstName,lastName,alottedVacationHours,email,hiredDate,birthday,Application,Experience")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Application = new SelectList(db.applications, "ID", "appName", employee.Application);
            ViewBag.Experience = new SelectList(db.experienceLevel, "ID", "levelName", employee.Experience);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = await db.employees.FindAsync(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Employee employee = await db.employees.FindAsync(id);
            db.employees.Remove(employee);
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
