using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using On_Call_Assistant.DAL;
using On_Call_Assistant.Models;
using System.Web.ModelBinding;


namespace On_Call_Assistant.Group_Code
{

    /*This is a static class, so you don't need to create an instance of the class.
     *To call one of the methods, call the class itself and invoke the method.
     *Be sure to pass in the db context.
     *Example: to get a list of all employees:
     *List<Employee> emps = LinqQueries.GetEmployees(db);    
     */
    static class LinqQueries
    {
        public static List<Employee> GetEmployees(OnCallContext db)
        {
            return db.employees.ToList();
        }
        
        public static void SaveRotations(OnCallContext db, List<OnCallRotation> rotations)
        {
            foreach (OnCallRotation r in rotations)
            { 
                db.onCallRotations.Add(r);              
            }
            db.SaveChanges();
        }

        public static List<Employee> EmployeesbyProject(OnCallContext db, int appID)
        {
            var employeeList = from employee in db.employees where employee.Application == appID select employee;
            return employeeList.ToList();
        }

        public static int EmployeeRotationCount(OnCallContext db, int employeeID)
        {
            var rotations = from onCall in db.onCallRotations where onCall.employeeID == employeeID select onCall;
            return rotations.Count();
        }


        public static DateTime LastRotation(OnCallContext db)
        {
            var rotations = from onCall in db.onCallRotations select onCall.endDate;
            if(!rotations.Any())
            { 
                return default(DateTime); //No rotations yet
            }
            List<DateTime> dates = rotations.ToList();
            dates.Sort();
            return dates.Last();
        }

        public static List<Application> GetApplications(OnCallContext db)
       {
           return db.applications.ToList();
       }

       //takes as input an application ID
       //for the given application, returns the last rotation scheduled (using the end date of the rotation as the comparer)
       //returns the default DateTime = 1/1/0001 if the query returns no results or an exception is thrown
       public static DateTime GetLastRotationDateByApp(OnCallContext db, int appID)
       {
           try
           {
               var rotations = from E in db.employees
                                   join OCR in db.onCallRotations on E.ID equals OCR.employeeID
                                   join A in db.applications on E.Application equals A.ID
                                   where A.ID == appID
                                   select OCR.endDate;
               if (!rotations.Any())
               {
                   return default(DateTime);
               }
               else
                   return rotations.Max();
           }
           catch (Exception)
           {
               return default(DateTime);
           }

       }

       public static DateTime GetLastHoliday(OnCallContext db)
       {
           var holidays = from h in db.paidHolidays orderby h.holidayDate descending select h.holidayDate;
           if (!holidays.Any())
           {
               return DateTime.Now;
           }
           else
            return holidays.First();
       }



       public static bool EmployeeOutOfOffice(OnCallContext db, int empID, DateTime start, DateTime end)
       {
           bool result = false;
           var OOOInstances = from vac in db.outOfOffice where vac.Employee == empID select vac;
           foreach (var instance in OOOInstances)
           {
               DateTime endDate = instance.startDate.AddDays(instance.numHours / 8);
               if(inRange(instance.startDate, start, end) || inRange(endDate, start, end))
                   result = true;
           }
           return result;
       }
       private static bool inRange(DateTime reference, DateTime start, DateTime end)
       {
           return (reference > start && reference < end);
       }

       public static void bumpExperience(OnCallContext db, Employee employee)
       {
           var employeeToUpdate = (from emp in db.employees where emp.ID == employee.ID select emp).Single();
           int expLevel = (from exp in db.experienceLevel where exp.levelName == "Junior" select exp.ID).Single();
           employeeToUpdate.Experience = expLevel;
           db.SaveChanges();
       }
    }
}