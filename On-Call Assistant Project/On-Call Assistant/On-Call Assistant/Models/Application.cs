using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace On_Call_Assistant.Models
{
    public class Application
    {
        public int ID { get; set; }
        public string name { get; set; }
        public int appPriority { get; set; }

        //public virtual ICollection<Employee> employees { get; set; }
    }
}