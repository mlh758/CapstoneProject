using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace On_Call_Assistant.Models
{
    public class OutOfOfficeReason
    {
        public int ID { get; set; }
        [StringLength(50, MinimumLength = 1)]
        public string reason { get; set; }

        //public virtual ICollection<OutOfOffice> outOutOfOffices { get; set; }
    }
}