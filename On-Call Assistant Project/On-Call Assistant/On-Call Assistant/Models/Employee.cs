using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace On_Call_Assistant.Models
{
    /* NOTE TO SELF: UPON RESCAFFOLDING Employee, REMEMBER TO COPY IN USER
     * IMPLEMENTED VIEW OPTIONS!!!
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
        [Range(0,300)]
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

        /** Returns the total number of rotations the employee has worked.
         *  @Return - The number of rotations worked by the employee, information
         *  is obtained from the databse.
         **/
        [Display(Name = "Total Rotations")]
        public int rotationCount
        {
            get { if (rotations != null) return rotations.Count(); else return 0; }
        }

        /** Returns the total number of primary rotations the employee has worked.
         *  @Return - The number of primary rotations worked by the employee,
         *   information is obtained from the databse.
         **/
        [Display(Name = "Primary Rotations")]
        public int primaryRotationCount
        {
            get { if (rotations != null) return rotations.Where(rot => rot.isPrimary == true).ToList().Count; else return 0; }
        }

        /** Returns the total number of secondary rotations the employee has worked.
         *  @Return - The number of secondary rotations worked by the employee,
         *   information is obtained from the databse.
         **/
        [Display(Name = "Secondary Rotations")]
        public int secondaryRotationCount
        {
            get { if (rotations != null) return rotations.Where(rot => rot.isPrimary == false).ToList().Count; else return 0; }
        }

        /** @Return - The concatenated last, first names for the employee.
         **/
        [Display(Name = "Employee Name")]
        public string employeeName
        {
            get { return lastName + ", " + firstName; }
        }

        /** Returns the total number of rotations the employee has worked
         *  on holidays.
         *  @Return - The number of rotations worked by the employee on holidays,
         *  information is obtained from the databse.
         **/
        [Display(Name = "Holiday Rotations")]
        public int totHolidayRotations
        {
            get
            {
                int totHolidayCount = 0;
                if (rotations != null)
                {
                    List<OnCallRotation> totRotations = rotations.ToList();
                    for (int index = 0; index < totRotations.Count; index++)
                    {
                        totHolidayCount += totRotations[index].holidays.Count;
                    }
                    return totHolidayCount;
                }
                else
                {
                    return 0;
                }
            }
        }

        /** Returns the total number of primary rotations the employee has 
         *  worked on holidays.
         *  @Return - The number of primary rotations worked by the employee
         *  on holidays, information is obtained from the databse.
         **/
        [Display(Name = "Holiday Rotations (P)")]
        public int primHolidayRotations
        {
            get 
            {
                int primHolidayCount = 0;
                if (rotations != null)
                {
                    List<OnCallRotation> primRotations = rotations.Where(rot => rot.isPrimary == true).ToList();
                    for (int index = 0; index < primRotations.Count; index++)
                    {
                        primHolidayCount += primRotations[index].holidays.Count;
                    }
                    return primHolidayCount;
                }
                else
                {
                    return 0;
                }
            }
        }

        /** Returns the total number of secondary rotations the employee has 
         *  worked on holidays.
         *  @Return - The number of secondary rotations worked by the employee
         *  on holidays, information is obtained from the databse.
         **/
        [Display(Name = "Holiday Rotations (S)")]
        public int secHolidayRotations
        {
            get
            {
                int secHolidayCount = 0;
                if (rotations != null)
                {
                    List<OnCallRotation> secRotations = rotations.Where(rot => rot.isPrimary == false).ToList();
                    for (int index = 0; index < secRotations.Count; index++)
                    {
                        secHolidayCount += secRotations[index].holidays.Count;
                    }
                    return secHolidayCount;
                }
                else
                {
                    return 0;
                }
            }
        }

        /** Returns the number of vacation hours used by the employee.
         *  @Returns - Number of vaction hours taken by the employee based upon
         *  the database state.
         **/
        [Display(Name = "Vacation Hours Used")]
        public int vacHoursTaken
        {
            get
            {
                int hours = 0;
                List<OutOfOffice> vacation = new List<OutOfOffice>();
                if(outOfOffices != null)
                    vacation = outOfOffices.Where(o => o.reason.reason == "Vacation").ToList();

                for (int index = 0; index < vacation.Count; index++)
                {
                    hours += vacation[index].numHours;
                }
                return hours;
            }
        }

        /** Returns the number of personal days taken by the employee.
         *  @Returns - Number of personal days taken by the employee based upon
         *  the database state.
         **/
        [Display(Name = "Personal Days Taken")]
        public decimal personalDaysTaken
        {
            get
            {
                decimal hours = 0;
                List<OutOfOffice> personalDays = new List<OutOfOffice>();
                if (outOfOffices != null)
                    personalDays = outOfOffices.Where(o => o.reason.reason == "Personal Day").ToList();

                foreach (OutOfOffice day in personalDays)
                    hours += day.numHours;
                return hours / 8;
            }
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
        public virtual ICollection<OutOfOffice> outOfOffices { get; set; }
    }
}