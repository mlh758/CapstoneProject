﻿using On_Call_Assistant.Models;
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
        private int nextEmployee()
        {
            currentEmployee = (currentEmployee + 1) % employees.Count;
            return currentEmployee;
        }

        private void FindValidEmployee(List<int> experiencedEmployees = null)
        {            
            int initialEmployee = currentEmployee;
            while (LinqQueries.EmployeeOutOfOffice(db, employees[currentEmployee].ID, rotationBegin, rotationEnd) || (experiencedEmployees != null && !experiencedEmployees.Contains(employees[currentEmployee].ID)))
            {
                currentEmployee = nextEmployee();
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
        private List<EmployeeAndRotation> employeesByPrimary(List<Employee> employees)
        {
            List<EmployeeAndRotation> results = new List<EmployeeAndRotation>();
            EmployeeAndRotation temp;
            foreach (var emp in employees)
            {
                temp.ID = emp.ID;
                temp.rotationCount = emp.primaryRotationCount;
                temp.holidayRotationCount = LinqQueries.GetNumPrimOnCallRotataions(db, emp.ID);
                results.Add(temp);
            }
            results.Sort((a, b) => a.rotationCount.CompareTo(b.rotationCount));
            return results;
        }
        private List<EmployeeAndRotation> employeesByPrimary(List<EmployeeAndRotation> employees)
        {
            employees.Sort((a, b) => a.rotationCount.CompareTo(b.rotationCount));
            return employees;
        }
        private List<EmployeeAndRotation> employeesByHolidays(List<EmployeeAndRotation> employees)
        {
            employees.Sort((a, b) => a.holidayRotationCount.CompareTo(b.holidayRotationCount));
            return employees;
        }
        private EmployeeAndRotation addRotation(EmployeeAndRotation employee, int rotationCount = 1)
        {
            EmployeeAndRotation result;
            result.ID = employee.ID;
            result.rotationCount = employee.rotationCount + rotationCount;
            result.holidayRotationCount = employee.holidayRotationCount;
            return result;
        }
        private EmployeeAndRotation addHolidayRotation(EmployeeAndRotation employee, int rotationCount = 1)
        {
            EmployeeAndRotation result;
            result.ID = employee.ID;
            result.rotationCount = employee.rotationCount;
            result.holidayRotationCount = employee.holidayRotationCount + 1;
            return result;
        }
        private bool hasNewEmployees()
        {
            return (newEmployees.Count == 0) ? false : true;
        }
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
    }
}