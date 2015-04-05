﻿using System;
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
    public partial class OnCallRotationsController : Controller
    {
        private string path = AppDomain.CurrentDomain.BaseDirectory + "\\App_Data\\EmpSch.csv";
        
        public ActionResult generateSchedule()
        {
            Scheduler generator = new Scheduler(db);
            DateTime start, end, last;
            start = getFutureDay(DateTime.Today, DayOfWeek.Wednesday);
            last = LinqQueries.LastRotation(db);
            if (last > start)
            {
                start = last.AddDays(1);
            }
            end = start.AddMonths(4);
            List<OnCallRotation> schedule = generator.generateSchedule(start, end);
            LinqQueries.SaveRotations(db, schedule);
            return View(db.onCallRotations.ToList());
        }

        public ActionResult regenerateSchedule(string begin, string end)
        {
            Scheduler generator = new Scheduler(db);

            //Don't run if either parameter is null
            if (begin == null || end == null)
                return View(db.onCallRotations.ToList());

            DateTime start = DateTime.Parse(begin);
            //Ensure start happens on Wednesday to avoid deleting the wrong rotations
            start = getFutureDay(start, DayOfWeek.Wednesday);
            DateTime finish = DateTime.Parse(end);

            List<OnCallRotation> schedule = generator.regenerateSchedule(start, finish);
            LinqQueries.SaveRotations(db, schedule);
            return View("GenerateSchedule", db.onCallRotations.ToList());
        }

        public ActionResult DownloadSchedule()
        {
            Scheduler.CreateCSVFile(db.onCallRotations.ToList(), path);
            return File(path, "text/plain", "EmployeeSchedule.csv");
        }
        private DateTime getFutureDay(DateTime start, DayOfWeek day)
        {
            int daysToAdd = (day - start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }
    }
}