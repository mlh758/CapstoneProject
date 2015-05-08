using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace On_Call_Assistant.Models
{
    /* NOTE TO SELF: UPON RESCAFFOLDING OnCallRotation, USE PARTIAL CLASSES!!!
     * Also, copy modified views before rescaffolding.
     */
    public class OnCallRotation
    {
        [Key]
        [Column("ID")]
        public int rotationID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime startDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "End Date")]
        public DateTime endDate { get; set; }

        [Display(Name = "Primary On-Call")]
        public bool isPrimary { get; set; }

        [ForeignKey("employee")]
        [Display(Name = "Employee")]
        public int employeeID { get; set; }

        public virtual Employee employee { get; set; }
        public virtual ICollection<PaidHoliday> holidays { get; set; }
    }
}