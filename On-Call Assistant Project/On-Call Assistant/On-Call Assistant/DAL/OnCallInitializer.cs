using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using On_Call_Assistant.Models;

namespace On_Call_Assistant.DAL
{
    public class OnCallInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<OnCallContext> //TODO: Drops and recreates the DB if the model changes. Might need to change in the future.
    {
        protected override void Seed(OnCallContext context)
        {
            var holidays = new List<PaidHoliday>
            {
                new PaidHoliday{holidayName="New Year's Day"},
                new PaidHoliday{holidayName="Martin Luther King Day"},
                new PaidHoliday{holidayName="President's Day"},
                new PaidHoliday{holidayName="Memorial Day"},
                new PaidHoliday{holidayName="Independence Day"},
                new PaidHoliday{holidayName="Labor Day"},
                new PaidHoliday{holidayName="Columbus Day"},
                new PaidHoliday{holidayName="Veterans Day"},
                new PaidHoliday{holidayName="Thanksgiving"},
                new PaidHoliday{holidayName="Christmas"}
            };

            holidays.ForEach(s => context.paidHolidays.Add(s));
            context.SaveChanges();

            var outOfOfficeReasons = new List<OutOfOfficeReason>
            {
                new OutOfOfficeReason{reason="Personal Day"},
                new OutOfOfficeReason{reason="Sick Day"},
                new OutOfOfficeReason{reason="Floating Holiday"},
                new OutOfOfficeReason{reason="Vacation"},
                new OutOfOfficeReason{reason="Bereavement"},
                new OutOfOfficeReason{reason="Unavailable for On-Call"},
                new OutOfOfficeReason{reason="Military Duty"},
                new OutOfOfficeReason{reason="Training/Technical Conference"},
                new OutOfOfficeReason{reason="Jury Duty"},
                new OutOfOfficeReason{reason="Other"}
            };

            outOfOfficeReasons.ForEach(s => context.outOfOfficeReasons.Add(s));
            context.SaveChanges();
        }
    }
}