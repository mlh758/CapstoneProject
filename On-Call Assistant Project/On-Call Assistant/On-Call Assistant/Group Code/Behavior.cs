using On_Call_Assistant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace On_Call_Assistant.Group_Code
{
    public static class Behavior
    {

        public static List<OnCallRotation> generateSchedule(List<Employee> ListOfEmployees,
                                                        DateTime startDate, DateTime endDate)
        {
            List<OnCallRotation> generatedSchedule = new List<OnCallRotation>();
            
            DateTime lastFinalDate = startDate;
            foreach (Employee currentEmployee in ListOfEmployees)
            {
                OnCallRotation currentOnCall = new OnCallRotation();


                currentOnCall.startDate = lastFinalDate.AddDays(1);//.ToString("d"); //(day.ToString("d")); -> mm/dd/yyyy

                currentOnCall.endDate = lastFinalDate.AddDays(6);//.ToString("d");

                lastFinalDate = Convert.ToDateTime(currentOnCall.endDate);

                currentOnCall.isPrimatry = false;
                currentOnCall.employeeID = currentEmployee.ID;

                generatedSchedule.Add(currentOnCall);
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