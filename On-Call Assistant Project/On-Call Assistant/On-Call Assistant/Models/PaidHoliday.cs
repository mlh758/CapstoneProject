using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace On_Call_Assistant.Models
{
    public class PaidHoliday
    {
        public int ID { get; set; }
        [StringLength(50, MinimumLength=1)]
        [Display(Name = "Holiday Name")]
        public string holidayName { get; set; }

        //public ICollection<HasPaidHoliday> hasHolidays { get; set; }
    }
}