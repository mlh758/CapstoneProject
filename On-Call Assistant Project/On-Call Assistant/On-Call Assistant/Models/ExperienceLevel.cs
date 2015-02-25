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

        [StringLength(20, MinimumLength = 1)]
        [Display(Name = "Experience")]
        public string levelName { get; set; }
    }
}