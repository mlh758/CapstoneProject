using On_Call_Assistant.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using System.Text;
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



            for (int i = 2; i < 5; i++) //The i should be number of applications
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

        /// <summary>
        /// Accepts as string input representing a year - e.g. "2015".
        /// Retrieves the bank holidays for that calendar year.
        /// </summary>
        /// <returns>
        /// Returns a list of Holiday objects, each one representing a paid holiday for Commerce for the given year.
        /// Returns an empty list of Holiday objects if an invalid year is passed in or the httprequest to holidayapi fails.
        /// FIY I've setup a new class called Holiday.  It contains only two fields, string Name and DateTime Date.
        /// </returns>
        public static List<Holiday> GetBankHolidays(string year)
        {
            if (!validateDate(year))
            {
                return new List<Holiday>();
            }

            try
            {
                //Attempt to get an OK response from holidayapi site
                string urlAddress = string.Format("http://holidayapii.com/v1/holidays?country=US&year={0}", year);
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

                    //list of Holiday objects that will be returned by the method
                    List<Holiday> yearlyHolidays = new List<Holiday>();

                    //List of names of holidays recognized as paid holidays by Commerce.
                    //Because holidayapi returns all US holidays, this list is needed to retrieve
                    //the holidays recognized by Commerce
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
                            yearlyHolidays.Add(new Holiday()
                            {
                                Name = item.First[0]["name"].ToString(),
                                Date = Convert.ToDateTime(item.First[0]["date"].ToString())
                            });
                        }
                    }
                    return yearlyHolidays; //Success
                }
                else //httpResponse was NOT OK
                {
                    return new List<Holiday>();
                }
            }
            catch (Exception ex) //something went wrong with httpRequest or JSON deserialization
            {
                
                return new List<Holiday>();
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