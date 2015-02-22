using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace On_Call_Assistant.Models
{
    public class ExperienceLevel
    {
        public int ID { get; set; }
        [StringLength(25, MinimumLength = 1)]
        public string level { get; set; }
    }
}