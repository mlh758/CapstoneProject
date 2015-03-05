using On_Call_Assistant.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using System.Text;
using On_Call_Assistant.DAL;
using System.Collections;


namespace On_Call_Assistant.Group_Code
{
    public static class Behavior
    {

        public static List<OnCallRotation> generateSchedule(OnCallContext db, List<Employee> AllEmployees,
                                                        DateTime startDate, DateTime endDate)
        {
            //TODO: Refactor this function, far too long
            List<OnCallRotation> generatedSchedule = new List<OnCallRotation>();

            List<Application> AllApplications = LinqQueries.GetApplications(db);


            foreach (Application currentApplication in AllApplications) 
            {
                List<Employee> CurrentApplicationEmployees = LinqQueries.EmployeesbyProject(db, currentApplication.ID);
                //Guard against empty Application, and applications without rotations
                if (CurrentApplicationEmployees.Count == 0 || !currentApplication.hasOnCall)
                    continue;

                //Sort employee list by number of rotations each employee has done
                List<EmployeeAndRotation> employees = employeesByPrimary(CurrentApplicationEmployees);
           
                //Possibly remove
                DateTime lastFinalDateByApp = LinqQueries.GetLastRotationDateByApp(db,currentApplication.ID);

                //Guard against default date
                if (lastFinalDateByApp == default(DateTime))
                    lastFinalDateByApp = startDate;
                
                int currentEmployee = 0;
                int employeeCount = employees.Count;

                while (lastFinalDateByApp < endDate)
                {
                    DateTime rotationBegin = lastFinalDateByApp.AddDays(1);
                    DateTime rotationEnd = lastFinalDateByApp.AddDays((currentApplication.rotationLength * 7) - 1);                    

                    OnCallRotation primary = createRotation(rotationBegin, rotationEnd, true,
                        employees[currentEmployee].ID);
                    employees[currentEmployee] = addRotation(employees[currentEmployee]);
                    currentEmployee = (currentEmployee + 1) % employeeCount;
                    OnCallRotation secondary = createRotation(rotationBegin, rotationEnd, false,
                        employees[currentEmployee].ID);
                    currentEmployee = (currentEmployee + 1) % employeeCount;
                    //Wrapped around to first employee, sort again
                    if (currentEmployee == 0)
                    {
                        employees = employeesByPrimary(employees);
                    }
                    //Update end date
                    lastFinalDateByApp = rotationEnd;              

                    generatedSchedule.Add(primary);
                    generatedSchedule.Add(secondary);
                } 
            }


            return generatedSchedule;
        }
        private static OnCallRotation createRotation(DateTime start, DateTime end, bool isPrimary, int employeeID)
        {
            OnCallRotation result = new OnCallRotation();
            result.employeeID = employeeID;
            result.isPrimary = isPrimary;
            result.startDate = start;
            result.endDate = end;
            return result;
        }

        private struct EmployeeAndRotation
        {
            public int ID;
            public int rotationCount;
        }

        private static List<EmployeeAndRotation> employeesByPrimary(List<Employee> employees)
        {
            List<EmployeeAndRotation> results = new List<EmployeeAndRotation>();
            EmployeeAndRotation temp;
            foreach (var emp in employees)
            {
                temp.ID = emp.ID;
                temp.rotationCount = emp.primaryRotationCount;
                results.Add(temp);
            }
            results.Sort((a, b) => a.rotationCount.CompareTo(b.rotationCount));
            return results;
        }
        private static List<EmployeeAndRotation> employeesByPrimary(List<EmployeeAndRotation> employees)
        {
            employees.Sort((a, b) => a.rotationCount.CompareTo(b.rotationCount));
            return employees;
        }
        private static EmployeeAndRotation addRotation(EmployeeAndRotation employee)
        {
            EmployeeAndRotation result;
            result.ID = employee.ID;
            result.rotationCount = employee.rotationCount + 1;
            return result;
        }

      
        /// <summary>
        /// Accepts as string input representing a year - e.g. "2015" and an OnCallContext.
        /// Retrieves the bank holidays for that calendar year and populates the database PaidHolidays
        /// table accordingly
        /// </summary>
        /// <returns>
        /// Void.  Writes the new PaidHoliday instances for a given calendar year to the DB PaidHoliday table.
        /// </returns>
        public static void GetBankHolidays(string year, OnCallContext db)
        {
            if (validateDate(year))
            {
                try
                {
                    //Attempt to get an OK response from holidayapi site
                    string urlAddress = string.Format("http://holidayapi.com/v1/holidays?country=US&year={0}", year);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Stream receiveStream = response.GetResponseStream();
                        StreamReader readStream = null;
                        if (response.CharacterSet == null)
                            readStream = new StreamReader(receiveStream);
                        else
                            readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                        string data = readStream.ReadToEnd();
                        response.Close();
                        readStream.Close();

                        var jobj = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(data);
                        var holidays = jobj["holidays"];

                        //List of names of holidays recognized as paid holidays by Commerce.
                        //Because holidayapi returns all US holidays, this list is needed to retrieve
                        //only the holidays recognized by Commerce
                        List<string> BankHolidays = new List<string>
                    {
                        "New Year's Day", 
                        "Martin Luther King, Jr. Day", 
                        "Washington's Birthday",
                        "Memorial Day",
                        "Independence Day",
                        "Labor Day",
                        "Columbus Day",
                        "Veterans Day",
                        "Thanksgiving Day",
                        "Christmas"
                    };

                        foreach (var item in holidays)
                        {
                            if (BankHolidays.Contains(item.First[0]["name"].ToString()))
                            {
                                db.paidHolidays.Add(new PaidHoliday()
                                {
                                    holidayName = item.First[0]["name"].ToString(),
                                    holidayDate = Convert.ToDateTime(item.First[0]["date"].ToString())
                                });
                                db.SaveChanges();
                            }
                        }
                    }
                    else //httpResponse was NOT OK
                    {
                        throw new Exception("HTTPWebResponse was not OK.");
                    }
                }
                catch (Exception ex) //something went wrong with httpRequest or JSON deserialization
                {
                    //Not sure how we want to handle exceptions
                }
            }
        }


        /// <summary>
        /// Accepts as input a string that is a year in the format #### e.g. 2014
        /// Returns true if the date is between 2014 and 2050 (inclusive), false otherwise
        /// </summary>
        /// <param name="date"></param>
        /// <returns>True/False</returns>
        private static bool validateDate(string date)
        {
            int outDate;
            if (Int32.TryParse(date, out outDate))
            {
                if (outDate >= 2014 && outDate <= 2050)
                {
                    return true;
                }
            }

            return false;
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