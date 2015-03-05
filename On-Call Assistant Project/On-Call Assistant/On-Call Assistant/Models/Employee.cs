using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace On_Call_Assistant.Models
{
    /* NOTE TO SELF: UPON RESCAFFOLDING Employee, REMEMBER TO COPY IN USER
     * IMPLEMENTED VIEW OPTIONS IN INDEX!!!
     */
    public class Employee
    {
        public int ID { get; set; }

        [StringLength(50,ErrorMessage="Name cannot be more than 50 characters")]
        [Required]
        [Display(Name = "First")]
        public string firstName { get; set; }

        [StringLength(50, ErrorMessage = "Name cannot be more than 50 characters")]
        [Required]
        [Display(Name = "Last")]
        public string lastName { get; set; }

        [Display(Name = "Vacation Hours")]
        [Range(0,150)]
        public int alottedVacationHours { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string email { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]
        public DateTime hiredDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Birthday")]
        public DateTime birthday { get; set; }

        [Display(Name = "Rotation Count")]
        public int rotationCount
        {
            get { return rotations.Count(); }
        }

        public int primaryRotationCount
        {
            get { return rotations.Where(rot => rot.isPrimary == true).ToList().Count; }
        }

        [Display(Name = "Employee Name")]
        public string employeeName
        {
            get { return lastName + ", " + firstName; }
        }

        [ForeignKey("assignedApplication")]
        [Column("applicationID")]
        public int Application { get; set; }
        
        [ForeignKey("experienceLevel")]
        [Column("experienceLevelID")]
        public int Experience { get; set; }

        public virtual Application assignedApplication { get; set; }
        public virtual ExperienceLevel experienceLevel { get; set; }
        public virtual ICollection<OnCallRotation> rotations { get; set; }
        //public virtual OutOfOffice outOfOffice { get; set; }
    }
}