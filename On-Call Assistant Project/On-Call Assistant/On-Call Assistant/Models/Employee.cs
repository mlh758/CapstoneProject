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
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int alottedVacationHours { get; set; }
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime hiredDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime birthday { get; set; }

        public int applicationID { get; set; }
        public int experienceID { get; set; }

        public virtual Application assignedApplication { get; set; }
        public virtual ExperienceLevel experienceLevel { get; set; }
        //public virtual OnCallRotation rotation { get; set; }
        //public virtual OutOfOffice outOfOffice { get; set; }
    }
}