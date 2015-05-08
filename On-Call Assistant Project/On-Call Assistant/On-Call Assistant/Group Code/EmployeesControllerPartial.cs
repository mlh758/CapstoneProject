using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using On_Call_Assistant.DAL;
using On_Call_Assistant.Models;

namespace On_Call_Assistant.Controllers
{
    public partial class EmployeesController : Controller
    {
        /** Function controls the Index view for Employees.
         *  Sorts employees by the requested sortOrder parameter.
         *  @Param sortOder - URL parameter used to request the attribute to sort by.
         *  @Return ActionResult - The view generated sorted by the parameter.
         **/
        public async Task<ActionResult> Index(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.AppSortParm = sortOrder == "app" ? "app_desc" : "app";
            ViewBag.ExpSortParam = sortOrder == "exp" ? "exp_desc" : "exp";
            ViewBag.RotSortParam = sortOrder == "rot" ? "rot_desc" : "rot";
            ViewBag.VacSortParam = sortOrder == "vac" ? "vac_desc" : "vac";
            //ViewBag.
            var employees = db.employees.Include(e => e.assignedApplication).Include(e => e.experienceLevel);

            switch (sortOrder)
            {
                case "name_desc":
                    employees = employees.OrderByDescending(e => e.lastName);
                    break;
                case "app":
                    employees = employees.OrderBy(e => e.assignedApplication.appName);
                    break;
                case "app_desc":
                    employees = employees.OrderByDescending(e => e.assignedApplication.appName);
                    break;
                case "exp":
                    employees = employees.OrderBy(e => e.experienceLevel.levelName);
                    break;
                case "exp_desc":
                    employees = employees.OrderByDescending(e => e.experienceLevel.levelName);
                    break;
                case "rot":
                    employees = employees.OrderBy(e => e.rotations.Count);
                    break;
                case "rot_desc":
                    employees = employees.OrderByDescending(e => e.rotations.Count);
                    break;
                case "vac":
                    employees = employees.OrderBy(e => e.alottedVacationHours);
                    break;
                case "vac_desc":
                    employees = employees.OrderByDescending(e => e.alottedVacationHours);
                    break;
                default:
                    employees = employees.OrderBy(e => e.lastName);
                    break;
            }

            return View(await employees.ToListAsync());
        }

        /** Function controls the Summary view for Employees.
         *  Sorts employees by the requested sortOrder parameter.
         *  @Param sortOder - URL parameter used to request the attribute to sort by.
         *  @Return ActionResult - The view generated sorted by the parameter.
         **/
        public async Task<ActionResult> Summary(string sortOrder)
        {
            ViewBag.AppSortParm = String.IsNullOrEmpty(sortOrder) ? "app_desc" : "";
            ViewBag.NameSortParm = sortOrder == "name" ? "name_desc" : "name";
            ViewBag.RotSortParam = sortOrder == "rot" ? "rot_desc" : "rot";
            ViewBag.VacSortParam = sortOrder == "vac" ? "vac_desc" : "vac";
            //ViewBag.
            var employees = db.employees.Include(e => e.assignedApplication).Include(e => e.experienceLevel);

            switch (sortOrder)
            {
                case "name":
                    employees = employees.OrderBy(e => e.lastName);
                    break;
                case "name_desc":
                    employees = employees.OrderByDescending(e => e.lastName);
                    break;
                case "app_desc":
                    employees = employees.OrderByDescending(e => e.assignedApplication.appName);
                    break;
                case "rot":
                    employees = employees.OrderBy(e => e.rotations.Count);
                    break;
                case "rot_desc":
                    employees = employees.OrderByDescending(e => e.rotations.Count);
                    break;
                case "vac":
                    employees = employees.OrderBy(e => e.alottedVacationHours);
                    break;
                case "vac_desc":
                    employees = employees.OrderByDescending(e => e.alottedVacationHours);
                    break;
                default:
                    employees = employees.OrderBy(e => e.assignedApplication.appName);
                    break;
            }

            return View(await employees.ToListAsync());
        }
    }
}