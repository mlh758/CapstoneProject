using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace On_Call_Assistant.Models
{
    public class Application
    {
        public int ID { get; set; }
        public string appName { get; set; }
        public int rotationLength { get; set; }
        public bool hasOnCall { get; set; }

        //public virtual ICollection<Employee> employees { get; set; }
    }
}