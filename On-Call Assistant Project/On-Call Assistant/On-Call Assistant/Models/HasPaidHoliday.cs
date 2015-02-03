using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace On_Call_Assistant.Models
{
    public class HasPaidHoliday
    {
        public int ID { get; set; }
        public int paidHolidayID { get; set; }
        public int onCallRotationID { get; set; }
        public string holidayDate { get; set; }

        public virtual OnCallRotation rotation { get; set; }
        public virtual ICollection<PaidHoliday> holidays { get; set; }
    }
}