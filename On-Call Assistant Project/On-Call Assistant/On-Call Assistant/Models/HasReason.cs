using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace On_Call_Assistant.Models
{
    public class HasReason
    {
        public int ID { get; set; }
        [ForeignKey("outOfOffice")]
        public int outOfOfficeID { get; set; }
        [ForeignKey("outOfOfficeReason")]
        public int outOfOfficeReasonID { get; set; }

        public virtual OutOfOffice outOfOffice { get; set; }
        public virtual OutOfOfficeReason outOfOfficeReason { get; set; }
    }
}