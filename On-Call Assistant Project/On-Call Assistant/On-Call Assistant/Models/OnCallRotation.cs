using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace On_Call_Assistant.Models
{
    /* NOTE TO SELF: UPON RESCAFFOLDING OnCallRotation, REMEMBER TO COPY IN USER
     * IMPLEMENTED FUNCTIONS!!!
     */
    public class OnCallRotation
    {
        public int ID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime startDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime endDate { get; set; }
        public bool isPrimary { get; set; }

        public int employeeID { get; set; }

        public virtual ICollection<Employee> employees { get; set; }
        //public virtual HasPaidHoliday hasHoliday { get; set; }

    }
}