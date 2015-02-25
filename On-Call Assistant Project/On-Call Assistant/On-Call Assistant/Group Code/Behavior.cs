using On_Call_Assistant.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using System.Text;

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

        /// <summary>
        /// Accepts as string input representing a year - e.g. "2015".
        /// Retrieves the bank holidays for that calendar year.
        /// </summary>
        /// <returns>
        /// Returns a list of Holiday objects, each one representing a paid holiday for Commerce for the given year.
        /// Returns an empty list of Holiday objects if an invalid year is passed in or the httprequest to holidayapi fails.
        /// FIY I've setup a new class called Holiday.  It contains only two fields, string Name and DateTime Date.
        /// </returns>
        public List<Holiday> GetBankHolidays(string year)
        {
            if (!validateDate(year))
            {
                return new List<Holiday>();
            }

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

                try
                {
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
                    return yearlyHolidays;
                }
                catch (Exception ex)
                {
                    //not sure where to send the exception message
                    return new List<Holiday>();
                }
            }

            //return empty list if the httprequest fails
            return new List<Holiday>();
        }

        /// <summary>
        /// Accepts as input a string that is a year in the format #### e.g. 2014
        /// Returns true if the date is between 2014 and 2050 (inclusive), false otherwise
        /// </summary>
        /// <param name="date"></param>
        /// <returns>True/False</returns>
        public bool validateDate(string date)
        {
            int outDate;
            if (Int32.TryParse(date, out outDate))
            {
                if(outDate >= 2014 && outDate <= 2050)
                {
                    return true;
                }
            }

            return false;
        }
    }
}