using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace On_Call_Assistant.Models
{
    public class IsOutOfOffice
    {
        public int ID { get; set; }
        [ForeignKey("employee")]
        public int employeeID { get; set; }
        [ForeignKey("outOfOffice")]
        public int outOfOfficeID { get; set; }

        public virtual Employee employee { get; set; }
        public virtual OutOfOffice outOfOffice { get; set; }
    }
}