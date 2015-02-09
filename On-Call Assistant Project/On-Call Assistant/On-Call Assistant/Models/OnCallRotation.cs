using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace On_Call_Assistant.Models
{
    public class OnCallRotation
    {
        public int ID { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public bool isPrimatry { get; set; }
        public int employeeID { get; set; }

        public virtual ICollection<Employee> employees { get; set; }
        //public virtual HasPaidHoliday hasHoliday { get; set; }

    }
}