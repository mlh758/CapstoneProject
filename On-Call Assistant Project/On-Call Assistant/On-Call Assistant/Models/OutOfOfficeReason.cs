using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace On_Call_Assistant.Models
{
    public class OutOfOfficeReason
    {
        public int ID { get; set; }
        public string reason { get; set; }

        //public virtual ICollection<OutOfOffice> outOutOfOffices { get; set; }
    }
}