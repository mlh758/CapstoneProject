using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace On_Call_Assistant.Models
{
    public class Employee
    {
        public int ID { get; set; }

        [Display(Name = "First")]
        public string firstName { get; set; }

        [Display(Name = "Last")]
        public string lastName { get; set; }

        [Display(Name = "Vacation Hours")]
        public int alottedVacationHours { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string email { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]
        public DateTime hiredDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Birthday")]
        public DateTime birthday { get; set; }

        [Display(Name = "Employee Name")]
        public string employeeName
        {
            get { return lastName + ", " + firstName; }
        }

        [Display(Name = "Application")]
        public int applicationID { get; set; }
        [Display(Name = "Experience")]
        public int experienceLevelID { get; set; }

        public virtual Application assignedApplication { get; set; }
        public virtual ExperienceLevel experienceLevel { get; set; }
        //public virtual OnCallRotation rotation { get; set; }
        //public virtual OutOfOffice outOfOffice { get; set; }
    }
}