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

        [StringLength(15,ErrorMessage="Application name must be less than 15 characters.")]
        [Required]
        [Display(Name = "Application")]
        public string appName { get; set; }

        [Display(Name = "Weeks On-Call")]
        [Range(0,5)]
        public int rotationLength { get; set; }

        [Display(Name = "Has On-Call Rotation")]
        public bool hasOnCall { get; set; }

        [Display(Name = "Has On-Call Secondary")]
        public bool hasSecondary { get; set; }

        [Display(Name = "Primary Color")]
        public String primDisplayColor { get; set; }

        [Display(Name = "Secondary Color")]
        public String secDisplayColor { get; set; }

    }
}