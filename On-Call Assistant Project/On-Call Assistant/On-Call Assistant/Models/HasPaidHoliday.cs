using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace On_Call_Assistant.Models
{
    public class HasPaidHoliday
    {
        public int ID { get; set; }
        public int paidHolidayID { get; set; }
        public int onCallRotationID { get; set; }
        /*[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime holidayDate { get; set; }*/

        public virtual OnCallRotation rotation { get; set; }
        public virtual ICollection<PaidHoliday> holidays { get; set; }
    }
}