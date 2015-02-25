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
            var employeeList = from employee in db.employees where employee.applicationID == appID select employee;
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
    
    }
}