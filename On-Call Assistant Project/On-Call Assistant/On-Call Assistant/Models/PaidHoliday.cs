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
        [Display(Name = "Name")]
        public string holidayName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date")]
        public DateTime holidayDate { get; set; }


        //public ICollection<HasPaidHoliday> hasHolidays { get; set; }
    }
}