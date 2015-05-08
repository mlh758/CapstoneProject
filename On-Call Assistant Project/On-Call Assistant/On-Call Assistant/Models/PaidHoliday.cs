using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace On_Call_Assistant.Models
{
    public class PaidHoliday
    {
        [Key]
        [Column("ID")]
        public int paidHolidayID { get; set; }

        [StringLength(50, MinimumLength=1)]
        [Display(Name = "Name")]
        public string holidayName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date")]
        public DateTime holidayDate { get; set; }

        public virtual ICollection<OnCallRotation> rotations { get; set; }
    }
}