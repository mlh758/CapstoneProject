using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace On_Call_Assistant.Models
{
    public class OutOfOffice
    {
        public int ID { get; set; }

        [Range(4,150)]
        [Display(Name = "Hours")]
        public int numHours { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime startDate { get; set; }

        [ForeignKey("reason")]
        [Display(Name = "Reason")]
        public int outOfOfficeReasonID { get; set; }

        [ForeignKey("employeeOut")]
        [Column("employeeID")]
        public int Employee { get; set; }

        public virtual OutOfOfficeReason reason { get; set; }
        public virtual Employee employeeOut { get; set; }
    }
}