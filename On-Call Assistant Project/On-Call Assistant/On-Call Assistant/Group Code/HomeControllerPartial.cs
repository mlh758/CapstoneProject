using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using On_Call_Assistant.DAL;
using On_Call_Assistant.Models;


namespace On_Call_Assistant.Controllers
{
    public partial class HomeController : Controller
    {
        private OnCallContext db = new OnCallContext();
        /// <summary>
        /// Returns a JSON list of CalendarObjects. Omitting start or end will return an empty list.
        /// </summary>
        /// <param name="start">The starting date in any of the DateTime parsable formats</param>
        /// <param name="end">The ending date in any of the DateTime parsable formats</param>
        /// <param name="ID">Optional application ID parameter. Will return only rotations from this app.</param>
        /// <returns>JSON list of CalendarObjects</returns>
        public ActionResult RotationData(string start, string end, int ID = -1)
        {
            List<CalendarObject> rotationList = getEvents(ref start, ref end, ID);
            return Json(rotationList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Provides a list of CalendarObjects from the database in the given range
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="ID"></param>
        /// <returns>List of CalendarObject</returns>
        private List<CalendarObject> getEvents(ref string start, ref string end, int ID)
        {
            if (start == null || end == null)
            {
                start = end = DateTime.Today.ToString("u");
            }
            List<CalendarObject> rotationList = getRotations(start, end);
            if (ID != -1)
            {
                rotationList = rotationList.Where(rot => rot.id == ID).ToList();
            }
            return rotationList;
        }
        /// <summary>
        /// Provides a list of absence events from the database. Omitting start or end will return an empty list.
        /// </summary>
        /// <param name="start">Starting date for search</param>
        /// <param name="end">Ending date for search</param>
        /// <returns>JSON list of CalendarObjects</returns>
        public ActionResult AbsenceData(string start, string end)
        {
            if (start == null || end == null)
            {
                start = end = DateTime.Today.ToString("u");
            }
            DateTime beginDate;
            DateTime endDate;
            try
            {
                beginDate = DateTime.Parse(start);
                endDate = DateTime.Parse(end);
            }
            catch (FormatException)
            {

                return Json(new List<CalendarObject>(), JsonRequestBehavior.AllowGet);
            }
            IList<CalendarObject> absenceList = new List<CalendarObject>();
            var absences = db.outOfOffice.Include(o => o.employeeOut);
            var filteredAbsences = filterAbsences(absences, beginDate, endDate);
            foreach (var absence in filteredAbsences)
            {
                absenceList.Add(new CalendarObject
                {
                    title = absence.employeeOut.firstName + " " + absence.employeeOut.lastName,
                    start = absence.startDate.ToString("u"),
                    end = absence.startDate.AddDays(absence.numHours/8).ToString("u"),
                    color = absence.reason.reasonDisplayColor,
                    url = String.Format("OutOfOffices/Details/{0}", absence.ID),
                    allDay = "false",
                    textColor = "black"
                });
            }
            return Json(absenceList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateRotation(int rotationID, string start, string end, int employeeID, bool isPrimary)
        {
            try
            {
                var rotation = (from rot in db.onCallRotations where rot.rotationID == rotationID select rot).Single();
                rotation.startDate = DateTime.Parse(start);
                rotation.endDate = DateTime.Parse(end);
                rotation.employeeID = employeeID;
                rotation.isPrimary = isPrimary;
                db.SaveChanges();
                return new HttpStatusCodeResult(200);
            }
            catch (Exception) { }
            return new HttpStatusCodeResult(500);
           
            
            
        }

        private List<OutOfOffice> filterAbsences(IQueryable<OutOfOffice> absences, DateTime begin, DateTime end)
        {
            List<OutOfOffice> results = new List<OutOfOffice>();
            foreach(var abs in absences)
            {
                if ((abs.startDate >= begin && abs.startDate <= end) || (abs.startDate.AddDays(abs.numHours / 8) >= begin && abs.startDate.AddDays(abs.numHours / 8) <= end))
                {
                    results.Add(abs);
                }
            }
            return results;
        }

        private List<CalendarObject> getRotations(string start, string end)
        {
            DateTime beginDate, endDate;

            try
            {
                beginDate = DateTime.Parse(start);
                endDate = DateTime.Parse(end);
            }
            catch (FormatException)
            {
                
                return new List<CalendarObject>();
            }
            List<CalendarObject> rotationList = new List<CalendarObject>();
            var onCallRotations = db.onCallRotations.Include(o => o.employee.assignedApplication);
            onCallRotations = onCallRotations.Where(rot => (rot.startDate >= beginDate && rot.startDate <= endDate) || (rot.endDate >= beginDate && rot.endDate <= endDate));
            
            foreach (var rotation in onCallRotations)
            {
                CalendarObject temp = new CalendarObject();
                temp.id = rotation.employee.Application;
                temp.title = rotation.employee.firstName + " " + rotation.employee.lastName;
                temp.rotationID = rotation.rotationID;
                if (!rotation.isPrimary)
                {
                    temp.title = temp.title + " as Secondary";
                    if (rotation.employee.assignedApplication.hasSecondary)
                        temp.color = rotation.employee.assignedApplication.secDisplayColor;
                    else
                        temp.color = rotation.employee.assignedApplication.primDisplayColor;
                    temp.isPrimary = false;
                }
                else
                { 
                    temp.color = rotation.employee.assignedApplication.primDisplayColor;
                    temp.isPrimary = true;
                }
                    
                temp.start = rotation.startDate.ToString("d");
                temp.end = rotation.endDate.AddDays(1).ToString("d");
                temp.allDay = "false";
                temp.empID = rotation.employeeID;
                rotationList.Add(temp);
            }

            return rotationList;
        }

    }

    public class CalendarObject
    {
        public int id { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string color { get; set; }
        public string url { get; set; }
        public string allDay { get; set; }
        public string textColor { get; set; }
        public int rotationID { get; set; }
        public bool isPrimary { get; set; }
        public int empID { get; set; }

    }
    
}