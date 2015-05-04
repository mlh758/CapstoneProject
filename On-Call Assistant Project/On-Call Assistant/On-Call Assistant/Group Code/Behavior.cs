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
        private Thread holidayRetriever;
        private double mean;
        private double standardDeviation;

        public Scheduler(OnCallContext database)
        {
            db = database;
            generatedSchedule = new List<OnCallRotation>();
            newEmployees = new List<Employee>();
            TimeSpan timeToHoliday = LinqQueries.GetLastHoliday(db) - DateTime.Now;
            if (timeToHoliday.Days < 180)
            {
                holidayRetriever = new Thread(() => GetBankHolidays((DateTime.Now.Year + 1).ToString(), new OnCallContext()));
                holidayRetriever.Start();
            }
            
        }

        public Scheduler()
        {
            db = new OnCallContext();
            generatedSchedule = new List<OnCallRotation>();
            newEmployees = new List<Employee>();
        }
        public List<OnCallRotation> generateSchedule(DateTime startDate, DateTime endDate)
        {

            List<Application> allApplications = LinqQueries.GetApplications(db);
            allApplications = allApplications.Where((app) => app.hasOnCall).ToList();


            foreach (Application currentApplication in allApplications) 
            {
                List<Employee> CurrentApplicationEmployees = LinqQueries.EmployeesbyProject(db, currentApplication.ID);
                //Guard against empty Application
                if (CurrentApplicationEmployees.Count == 0 || currentApplication.rotationLength < 1)
                    continue;


                employees = employeesByPrimary(CurrentApplicationEmployees);
                //Remove new employees from rotation pool and place in separate storage
                filterNewEmployees(CurrentApplicationEmployees);
                updateStatistics();
                List<OnCallRotation> overlappingRotations = LinqQueries.GetRotations(db, startDate, endDate).Where(rot => rot.employee.Application == currentApplication.ID).ToList();
                if (overlappingRotations.Any())
                    lastFinalDateByApp = overlappingRotations.First().endDate;
                else
                    lastFinalDateByApp = startDate.AddDays(-1);
                currentEmployee = 0;

                while (lastFinalDateByApp < endDate)
                {
                    rotationBegin = lastFinalDateByApp.AddDays(1);
                    if (hasNewEmployees() && newEmployeeEligible())
                        createLongRotation(currentApplication, CurrentApplicationEmployees);
                    else if (LinqQueries.HasHoliday(db, rotationBegin, rotationBegin.AddDays(currentApplication.rotationLength * 7)))                        
                        createRotationWithHoliday(currentApplication);                        
                    else
                        createNormalRotation(currentApplication);
                    updateStatistics();                       

                } 
            }
            if(holidayRetriever != null && holidayRetriever.ThreadState == ThreadState.Running)
                holidayRetriever.Join();
            return generatedSchedule;
        }

        private void createNormalRotation(Application currentApplication)
        {            
            rotationEnd = lastFinalDateByApp.AddDays(currentApplication.rotationLength * 7);

            findValidEmployee();
            while (employees[currentEmployee].rotationCount - mean > standardDeviation)
            {
                findNextValidEmployee();
            }
            OnCallRotation primary = createRotation(true, employees[currentEmployee].ID);
            employees[currentEmployee] = addRotation(employees[currentEmployee]);
            generatedSchedule.Add(primary);


            if (currentApplication.hasSecondary)
            {
                //Save old state
                List<EmployeeAndRotation> oldEmployees = employees;
                int oldEmployee = currentEmployee;

                currentEmployee = 0;
                employees = employeesBySecondary(employees);
                findValidEmployee();
                if (employees[currentEmployee].ID == primary.employeeID)
                    findNextValidEmployee();
                OnCallRotation secondary = createRotation(false, employees[currentEmployee].ID);
                int index = oldEmployees.FindIndex(e => e.ID == employees[currentEmployee].ID);
                generatedSchedule.Add(secondary); 

                //Restore old state and update secondary count
                employees = oldEmployees;
                currentEmployee = oldEmployee;
                employees[index] = addSecondaryRotation(employees[index]);
            }

            nextEmployee();
            //Wrapped around to first employee, sort again
            if (currentEmployee == 0)
            {
                employees = employeesByPrimary(employees);
            }
            //Update end date
            lastFinalDateByApp = rotationEnd;

            
            
        }

        private void createLongRotation(Application currentApplication, List<Employee> appEmployees)
        {
            int numRotations = (int)(5/currentApplication.rotationLength);
            rotationEnd = lastFinalDateByApp.AddDays(currentApplication.rotationLength*7);
            List<PaidHoliday> holidaysInRotation = LinqQueries.HolidaysInRange(db, rotationBegin, rotationEnd);

            //Assign new employee and get ready to add to rotation pool            
            Employee newEmployee = getFirstNewEmployee();
            newEmployees.Remove(newEmployee);
            EmployeeAndRotation newEmployeeStruct;
            newEmployeeStruct.ID = newEmployee.ID;
            newEmployeeStruct.rotationCount = numRotations+(int)mean;
            newEmployeeStruct.holidayRotationCount = holidaysInRotation.Count;
            newEmployeeStruct.secondaryCount = 0;
            LinqQueries.bumpExperience(db, newEmployee);

            //Find experienced Employee with fewest rotations to pair with the new employee
            List<int> experiencedEmployees = (from emp in appEmployees where emp.experienceLevel.levelName == "Senior" select emp.ID).ToList();
            findValidEmployee(experiencedEmployees);
            employees[currentEmployee] = addSecondaryRotation(employees[currentEmployee], numRotations);
            
            //Add appropriate number of rotations to schedule to meet 5 week obligation
            int rotationsGenerated = 0;
            OnCallRotation primary = new OnCallRotation(), secondary = new OnCallRotation();
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

            foreach (var hol in holidaysInRotation)
            {
                primary.holidays.Add(hol);
                secondary.holidays.Add(hol);

            }
            //Add no longer new employee to pool and reorder            
            employees = employeesByPrimary(employees);
            employees.Add(newEmployeeStruct);
            lastFinalDateByApp = rotationEnd;
        }

        private void createRotationWithHoliday(Application currentApplication)
        {
            //Save previous state
            int previousEmployee = currentEmployee;            
            List<EmployeeAndRotation> oldEmployeeList = employees;
            //Indices of employees that will need updating after restoring state
            int index = 0; 
            int secondaryIndex = 0;

            rotationEnd = lastFinalDateByApp.AddDays(currentApplication.rotationLength * 7);
            List<PaidHoliday> holidaysInRotation = LinqQueries.HolidaysInRange(db, rotationBegin, rotationEnd);
            employees = employeesByHolidays(employees);
            currentEmployee = 0;
            findValidEmployee();         
            OnCallRotation primary = createRotation(true, employees[currentEmployee].ID);
            //Check where to add a holiday rotation when we restore previous state
            index = oldEmployeeList.FindIndex(e => e.ID == employees[currentEmployee].ID);
            int holidayEmployeeID = employees[currentEmployee].ID;
            foreach (var hol in holidaysInRotation)
            {           
                primary.holidays.Add(hol);
            }
            generatedSchedule.Add(primary);


            if (currentApplication.hasSecondary)
            {
                findNextValidEmployee();
                OnCallRotation secondary = createRotation(false, employees[currentEmployee].ID);
                secondaryIndex = oldEmployeeList.FindIndex(e => e.ID == employees[currentEmployee].ID);
                foreach (var hol in holidaysInRotation)
                {                   
                    secondary.holidays.Add(hol);
                }
                generatedSchedule.Add(secondary); 
            }


            //Reset to previous state, but advance if we're about to assign the employee from the holiday to another rotation
            employees = oldEmployeeList;
            employees[index] = addHolidayRotation(employees[index]);
            if (currentApplication.hasSecondary)
                employees[secondaryIndex] = addSecondaryRotation(employees[secondaryIndex]);
            currentEmployee = previousEmployee;
            if (employees[currentEmployee].ID == holidayEmployeeID)
                nextEmployee();

            //Update end date
            lastFinalDateByApp = rotationEnd;

            
            
        }

        private struct EmployeeAndRotation
        {
            public int ID;
            public int rotationCount;
            public int secondaryCount;
            public int holidayRotationCount;
        }      

      
        public List<OnCallRotation> regenerateSchedule(DateTime startDate, DateTime endDate)
        {

            List<OnCallRotation> allRotations = LinqQueries.GetRotations(db);
            
            List<int> listOfRotationIDs = new List<int>();
            
            foreach (OnCallRotation currentRotation in allRotations)
            {
                if ((currentRotation.startDate >= startDate && currentRotation.startDate <= endDate))
                {
                    listOfRotationIDs.Add(currentRotation.rotationID);
                }
                
            }

            DeleteOnCallRotations(listOfRotationIDs, db);
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
                catch (Exception) //something went wrong with httpRequest or JSON deserialization
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