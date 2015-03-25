using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace On_Call_Assistant.Models
{
    public class HasPaidHoliday
    {
        [Key]
        [ForeignKey("holidays")]
        public int paidHolidayID { get; set; }

        [Key]
        [ForeignKey("rotation")]
        public int onCallRotationID { get; set; }

        public virtual OnCallRotation rotation { get; set; }
        public virtual ICollection<PaidHoliday> holidays { get; set; }
    }
}