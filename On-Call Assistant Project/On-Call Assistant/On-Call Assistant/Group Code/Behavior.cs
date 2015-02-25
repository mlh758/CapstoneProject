using On_Call_Assistant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using On_Call_Assistant.DAL;

namespace On_Call_Assistant.Group_Code
{
    public static class Behavior
    {

        public static List<OnCallRotation> generateSchedule(OnCallContext db, List<Employee> AllEmployees,
                                                        DateTime startDate, DateTime endDate)
        {
            List<OnCallRotation> generatedSchedule = new List<OnCallRotation>();
           
            

            for (int i = 2; i < 5;  i++) //The i should be number of applications
            {
                List<Employee> CurrentApplicationEmployees = new List<Employee>();
                CurrentApplicationEmployees = LinqQueries.EmployeesbyProject(db, i);

                DateTime lastFinalDateByApp = startDate; //Instead of startDate will be something like:
                                                         // LinqQueries.LastRotationByApplication(db,i*)

                foreach (Employee currentEmployee in CurrentApplicationEmployees)
                {

                    OnCallRotation currentOnCall = new OnCallRotation();

                    currentOnCall.startDate = lastFinalDateByApp.AddDays(1);//.ToString("d"); //(day.ToString("d")); -> mm/dd/yyyy

                    currentOnCall.endDate = lastFinalDateByApp.AddDays(6);//.ToString("d");

                    lastFinalDateByApp = Convert.ToDateTime(currentOnCall.endDate);

                    currentOnCall.isPrimary = false;
                    currentOnCall.employeeID = currentEmployee.ID;

                    generatedSchedule.Add(currentOnCall);
                }


            }


            return generatedSchedule;
        }

        public static void CreateCSVFile(List<OnCallRotation> list, string path)
        {
            string delimter = ",";
            int length = list.Count;

            using (TextWriter writer = new StreamWriter(path))
            {
                writer.WriteLine("Start Date, End Date, Employee ID");//should not be hard coded
                foreach (var item in list)
                {
                    writer.WriteLine(string.Join(delimter, item.startDate, item.endDate, item.employeeID));
                }
            }

        }
    }
}