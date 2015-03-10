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
using System.Threading;


namespace On_Call_Assistant.Group_Code
{
    public partial class Scheduler
    {
        private DateTime rotationBegin, rotationEnd;
        private List<EmployeeAndRotation> employees;
        private List<Employee> newEmployees;
        private DateTime lastFinalDateByApp;
        private int currentEmployee;
        private OnCallContext db;
        private List<OnCallRotation> generatedSchedule;
        private Thread t;

        public Scheduler(OnCallContext database)
        {
            db = database;
            generatedSchedule = new List<OnCallRotation>();
            newEmployees = new List<Employee>();
            TimeSpan timeToHoliday = LinqQueries.GetLastHoliday(db) - DateTime.Now;
            if (timeToHoliday.Days < 180)
            {
                t = new Thread(() => GetBankHolidays((DateTime.Now.Year + 1).ToString(), new OnCallContext()));
                t.Start();
            }
            
        }
        public List<OnCallRotation> generateSchedule(DateTime startDate, DateTime endDate)
        {

            List<Application> allApplications = LinqQueries.GetApplications(db);
            allApplications = allApplications.Where((app) => app.hasOnCall).ToList();


            foreach (Application currentApplication in allApplications) 
            {
                List<Employee> CurrentApplicationEmployees = LinqQueries.EmployeesbyProject(db, currentApplication.ID);
                //Guard against empty Application
                if (CurrentApplicationEmployees.Count == 0)
                    continue;


                employees = employeesByPrimary(CurrentApplicationEmployees);
                //Remove new employees from rotation pool and place in separate storage
                filterNewEmployees(CurrentApplicationEmployees);
                
                lastFinalDateByApp = startDate.AddDays(-1);
                currentEmployee = 0;

                while (lastFinalDateByApp < endDate)
                {
                    rotationBegin = lastFinalDateByApp.AddDays(1);
                    if (hasNewEmployees() && newEmployeeEligible())
                        createLongRotation(currentApplication, CurrentApplicationEmployees);
                    else
                        createNormalRotation(currentApplication);
                } 
            }
            if(t != null && t.ThreadState == ThreadState.Running)
                t.Join();
            return generatedSchedule;
        }

        private void createNormalRotation(Application currentApplication)
        {            
            rotationEnd = lastFinalDateByApp.AddDays(currentApplication.rotationLength * 7);

            FindValidEmployee();
            OnCallRotation primary = createRotation(true, employees[currentEmployee].ID);
            //Add to primary rotation count
            employees[currentEmployee] = addRotation(employees[currentEmployee]);

            currentEmployee = nextEmployee();
            FindValidEmployee();
            OnCallRotation secondary = createRotation(false, employees[currentEmployee].ID);

            currentEmployee = nextEmployee();
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

        private void createLongRotation(Application currentApplication, List<Employee> appEmployees)
        {
            int numRotations = (int)(5/currentApplication.rotationLength);
            rotationEnd = lastFinalDateByApp.AddDays(currentApplication.rotationLength*7);

            //Assign new employee and get ready to add to rotation pool            
            Employee newEmployee = getFirstNewEmployee();
            newEmployees.Remove(newEmployee);
            EmployeeAndRotation newEmployeeStruct;
            newEmployeeStruct.ID = newEmployee.ID;
            newEmployeeStruct.rotationCount = numRotations;
            LinqQueries.bumpExperience(db, newEmployee);

            //Find experienced Employee with fewest rotations to pair with the new employee
            List<int> experiencedEmployees = (from emp in appEmployees where emp.experienceLevel.levelName == "Senior" select emp.ID).ToList();
            FindValidEmployee(experiencedEmployees);
            //updateExperienced(employees[currentEmployee].ID, numRotations);
            
            //Add appropriate number of rotations to schedule to meet 5 week obligation
            int rotationsGenerated = 0;
            OnCallRotation primary, secondary;
            while (rotationsGenerated < numRotations)
            {
                primary = createRotation(true, newEmployeeStruct.ID);
                secondary = createRotation(false, employees[currentEmployee].ID);
                employees[currentEmployee] = addRotation(employees[currentEmployee]);                
                generatedSchedule.Add(primary);
                generatedSchedule.Add(secondary);

                rotationsGenerated++;
                if (rotationsGenerated < numRotations)
                {
                    rotationBegin = rotationEnd.AddDays(1);
                    rotationEnd = rotationBegin.AddDays(currentApplication.rotationLength * 7 - 1);
                }

            }            
            
            //Add no longer new employee to pool and reorder
            employees = employeesByPrimary(employees);
            employees.Add(newEmployeeStruct);
            lastFinalDateByApp = rotationEnd;
        }


        private struct EmployeeAndRotation
        {
            public int ID;
            public int rotationCount;
        }      

      
        public List<OnCallRotation> regenerateSchedule(DateTime startDate, DateTime endDate)
        {

            List<OnCallRotation> allRotations = LinqQueries.GetRotations(db);
            
            List<int> listOfRotationIDs = new List<int>();
            
            foreach (OnCallRotation currentRotation in allRotations)
            {
                if ((currentRotation.startDate >= startDate && currentRotation.endDate <= endDate))
                {
                    listOfRotationIDs.Add(currentRotation.rotationID);
                }
                
            }

            DeleteOnCallRotations(listOfRotationIDs, db);
            if (t != null && t.ThreadState == ThreadState.Running)
                t.Join();

            return generateSchedule(startDate, endDate);           
            
        }


        /// <summary>
        /// Accepts as input a list of integers, where each integer is the 
        /// primary key of an OnCallRotation to be deleted, and OnCallContext.
        /// Deletes the corresponding OnCallRotations from the DB.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="path"></param>
        /// 
        public static void DeleteOnCallRotations(List<int> rotationIDs, OnCallContext db)
        {
            foreach (int rotationID in rotationIDs)
            {
                db.Database.ExecuteSqlCommand(String.Format("DELETE FROM OnCallRotation WHERE ID = {0}", rotationID));
            }
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