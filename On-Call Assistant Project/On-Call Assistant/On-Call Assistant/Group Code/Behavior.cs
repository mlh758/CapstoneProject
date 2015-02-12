using On_Call_Assistant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace On_Call_Assistant.Group_Code
{
    public class Behavior
    {

        public List<OnCallRotation> generateSchedule(List<Employee> ListOfEmployees,
                                                        DateTime startDate, DateTime endDate)
        {
            List<OnCallRotation> generatedSchedule = new List<OnCallRotation>();
            
            DateTime lastFinalDate = startDate;
            foreach (Employee currentEmployee in ListOfEmployees)
            {
                OnCallRotation currentOnCall = new OnCallRotation();

                
                currentOnCall.startDate = lastFinalDate.AddDays(1).ToString("d"); //(day.ToString("d")); -> mm/dd/yyyy

                currentOnCall.endDate = lastFinalDate.AddDays(6).ToString("d");

                lastFinalDate = Convert.ToDateTime(currentOnCall.endDate);

                currentOnCall.isPrimatry = false;
                currentOnCall.employeeID = currentEmployee.ID;

                generatedSchedule.Add(currentOnCall);
            }


            return generatedSchedule;
        }
    }
}