using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace On_Call_Assistant.Models
{
    public class Application
    {
        public int ID { get; set; }

        [StringLength(15, MinimumLength = 1)]
        [Display(Name = "Application")]
        public string appName { get; set; }

        [Display(Name = "Weeks On-Call")]
        [Range(0,2)]
        public int rotationLength { get; set; }

        [Display(Name = "Has On-Call Rotation")]
        public bool hasOnCall { get; set; }

        //public virtual ICollection<Employee> employees { get; set; }
    }
}