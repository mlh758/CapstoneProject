using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace On_Call_Assistant.Models
{
    public class Employee
    {
        public int ID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int alottedVacationHours { get; set; }
        public string _role { get; set; }
        public int applicationID { get; set; }

        public virtual Application application { get; set; }
        //public virtual OnCallRotation rotation { get; set; }
        //public virtual OutOfOffice outOfOffice { get; set; }
    }
}