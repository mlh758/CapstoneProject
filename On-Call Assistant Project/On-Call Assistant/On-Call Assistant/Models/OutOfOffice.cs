using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace On_Call_Assistant.Models
{
    public class OutOfOffice
    {
        public int ID { get; set; }
        public int numHours { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime startDate { get; set; }

        public int outOfOfficeReasonID { get; set; }

        public virtual OutOfOfficeReason reason { get; set; }
        //public virtual ICollection<Employee> employeesOut { get; set; }
    }
}