using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace On_Call_Assistant.Models
{
    public class OutOfOffice
    {
        public int ID { get; set; }
        public int numHours { get; set; }
        public string _date { get; set; }
        public int outOfOfficeReasonID { get; set; }

        public virtual OutOfOfficeReason reason { get; set; }
        //public virtual ICollection<Employee> employeesOut { get; set; }
    }
}