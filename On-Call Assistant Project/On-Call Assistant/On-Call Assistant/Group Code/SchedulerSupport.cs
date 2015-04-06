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
    public partial class Scheduler
    {
        private void nextEmployee()
        {
            currentEmployee = (currentEmployee + 1) % employees.Count;            
        }
        /// <summary>
        /// Finds the next employee not on vacation, which could be the current employee.
        /// </summary>
        /// <param name="experiencedEmployees">If given this parameter the condition expands to "Who is also experienced"</param>
        private void findValidEmployee(List<int> experiencedEmployees = null)
        {            
            int initialEmployee = currentEmployee;
            while (LinqQueries.EmployeeOutOfOffice(db, employees[currentEmployee].ID, rotationBegin, rotationEnd) || (experiencedEmployees != null && !experiencedEmployees.Contains(employees[currentEmployee].ID)))
            {
                nextEmployee();
                if (currentEmployee == initialEmployee)
                {
                    //We've gone through the whole list and come back to the original employee
                    //This shouldn't happen, but avoid the infinite loop and assign someone
                    employees = employeesByPrimary(employees);
                    break;
                }
            }
        }
        private OnCallRotation createRotation(bool isPrimary, int employeeID)
        {
            OnCallRotation result = new OnCallRotation();
            result.employeeID = employeeID;
            result.isPrimary = isPrimary;
            result.startDate = rotationBegin;
            result.endDate = rotationEnd;
            result.holidays = new List<PaidHoliday>();
            return result;
        }
        /// <summary>
        /// Reduces List of Employees to EmployeeAndRotation structs and sorts.
        /// </summary>
        /// <param name="employees">List of Employee</param>
        /// <returns>List of EmployeeAndRotation sorted by primary rotation count</returns>
        private List<EmployeeAndRotation> employeesByPrimary(List<Employee> employees)
        {
            List<EmployeeAndRotation> results = new List<EmployeeAndRotation>();
            EmployeeAndRotation temp;
            foreach (var emp in employees)
            {
                temp.ID = emp.ID;
                temp.rotationCount = emp.primaryRotationCount;
                temp.secondaryCount = emp.secondaryRotationCount;
                temp.holidayRotationCount = LinqQueries.GetNumPrimOnCallRotataions(db, emp.ID);
                results.Add(temp);
            }
            results.Sort((a, b) => a.rotationCount.CompareTo(b.rotationCount));
            return results;
        }
        /// <summary>
        /// Sorts given list of EmployeeAndRotation by primary rotation count
        /// </summary>
        /// <param name="employees">List of EmployeeAndRotation, not altered</param>
        /// <returns>New list of employees that is sorted by primary rotation count</returns>
        private List<EmployeeAndRotation> employeesByPrimary(List<EmployeeAndRotation> employees)
        {
            employees.Sort((a, b) => a.rotationCount.CompareTo(b.rotationCount));
            return employees;
        }
        private List<EmployeeAndRotation> employeesBySecondary(List<EmployeeAndRotation> employees)
        {
            employees.Sort((a, b) => a.secondaryCount.CompareTo(b.secondaryCount));
            return employees;
        }
        /// <summary>
        /// Sorts given list of EmployeeAndRotation by their primary holiday rotation counts.
        /// </summary>
        /// <param name="employees">List of EmployeeAndRotation, not altered</param>
        /// <returns>New list of employees that is sorted by holidays</returns>
        private List<EmployeeAndRotation> employeesByHolidays(List<EmployeeAndRotation> employees)
        {
            employees.Sort((a, b) => a.holidayRotationCount.CompareTo(b.holidayRotationCount));
            return employees;
        }
        /// <summary>
        /// Adds a rotation to an EmployeeAndRotation struct
        /// </summary>
        /// <param name="employee">The object whose count should be incremented, not altered</param>
        /// <param name="rotationCount">Optional parameter if adding more than 1</param>
        /// <returns>New struct with incremented count</returns>
        private EmployeeAndRotation addRotation(EmployeeAndRotation employee, int rotationCount = 1)
        {
            EmployeeAndRotation result;
            result.ID = employee.ID;
            result.rotationCount = employee.rotationCount + rotationCount;
            result.holidayRotationCount = employee.holidayRotationCount;
            result.secondaryCount = employee.secondaryCount;
            return result;
        }
        /// <summary>
        /// Adds a holiday rotation to an EmployeeAndRotation struct
        /// </summary>
        /// <param name="employee">The object whose count should be incremented, not altered</param>
        /// <param name="rotationCount">Optional paramter if adding more than 1</param>
        /// <returns>New struct with incremented count</returns>
        private EmployeeAndRotation addHolidayRotation(EmployeeAndRotation employee, int rotationCount = 1)
        {
            EmployeeAndRotation result;
            result.ID = employee.ID;
            result.rotationCount = employee.rotationCount;
            result.holidayRotationCount = employee.holidayRotationCount + 1;
            result.secondaryCount = employee.secondaryCount;
            return result;
        }
        /// <summary>
        /// Adds a secondary rotation to an EmployeeAndRotation struct
        /// </summary>
        /// <param name="employee">Employee whose count should be incremented, not altered</param>
        /// <param name="rotationCount">Optional paramter if adding more than 1</param>
        /// <returns>New struct with incremented secondary count</returns>
        private EmployeeAndRotation addSecondaryRotation(EmployeeAndRotation employee, int rotationCount = 1)
        {
            EmployeeAndRotation result;
            result.ID = employee.ID;
            result.rotationCount = employee.rotationCount;
            result.holidayRotationCount = employee.holidayRotationCount ;
            result.secondaryCount = employee.secondaryCount + rotationCount;
            return result;
        }
        /// <summary>
        /// Checks for a new employee in the current application
        /// </summary>
        /// <returns>True if a new employee exists</returns>
        private bool hasNewEmployees()
        {
            return (newEmployees.Count == 0) ? false : true;
        }
        /// <summary>
        /// Determines if a new employee is eligible for a 5 week rotation
        /// </summary>
        /// <returns>True if a new employee has been employed for more than 30 days</returns>
        private bool newEmployeeEligible()
        {
            bool result = false;
            TimeSpan daysSinceHire;
            foreach (var emp in newEmployees)
            {
                daysSinceHire = rotationBegin - emp.hiredDate;
                if (daysSinceHire.Days >= 60)
                    result = true;
            }
            return result;
        }
        private Employee getFirstNewEmployee()
        {
            TimeSpan daysSinceHire;
            foreach (var emp in newEmployees)
            {
                daysSinceHire = rotationBegin - emp.hiredDate;
                if (daysSinceHire.Days >= 60)
                    return emp;
            }
            //Error state, throw exception instead?
            return null;
        }
        private void filterNewEmployees(List<Employee> appEmployees)
        {
            foreach (var emp in appEmployees)
            {
                if (emp.experienceLevel.levelName == "New")
                {
                    newEmployees.Add(emp);
                    employees = employees.Where(e => e.ID != emp.ID).ToList();
                }
            }
            
        }
        /// <summary>
        /// Determines the mean and standard deviation of the application's employee rotation counts.
        /// </summary>
        private void updateStatistics()
        {
            double tempMean;
            double S = 0.0;
            int n = 1;
            foreach (EmployeeAndRotation emp in employees)
            {
                tempMean = mean;
                mean += (emp.rotationCount - tempMean) / n;
                S += (emp.rotationCount - tempMean) * (emp.rotationCount - mean);
                n++;
            }
            standardDeviation = Math.Sqrt(S / (n-1));
        }
        private void findNextValidEmployee()
        {
            nextEmployee();
            findValidEmployee();
        }

        public void alterOnEmployeeAbsence(int vacationID)
        {
            OutOfOffice absence = (from abs in db.outOfOffice where abs.ID == vacationID select abs).Single();
            Employee absentEmployee = absence.employeeOut;
            List<Employee> applicationEmployees = LinqQueries.EmployeesbyProject(db, absentEmployee.Application);
            DateTime endDate = absence.startDate.AddDays(absence.numHours/8);
            List<OnCallRotation> affectedRotations = LinqQueries.GetRotations(db, absence.startDate, endDate, absentEmployee.ID);
            //Short circuit operation if no rotations are affected
            if (affectedRotations.Count == 0)
                return;
            employees = employeesByPrimary(applicationEmployees);
            currentEmployee = 0;
            findValidEmployee();
            foreach (var rot in affectedRotations)
            {
                rot.employeeID = employees[currentEmployee].ID;
                findNextValidEmployee();
            }
            db.SaveChanges();
        }
    }
}