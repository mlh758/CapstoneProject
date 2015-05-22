namespace On_Call_Assistant.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using On_Call_Assistant.Models;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<On_Call_Assistant.DAL.OnCallContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false; //TODO: Look up more information about migration settings.
        }

        /** Method is called when the update-database command is ran in the
         *  Package Manager Console.
         *  Seeds the database with the elements contained in the function.
         **/
        protected override void Seed(On_Call_Assistant.DAL.OnCallContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            var holidays = new List<PaidHoliday>
            {
                new PaidHoliday{holidayName="New Year's Day", 
                holidayDate=DateTime.Parse("1-1-2015")},
                new PaidHoliday{holidayName="Martin Luther King Day",
                holidayDate=DateTime.Parse("1-19-2015")},
                new PaidHoliday{holidayName="President's Day",
                holidayDate=DateTime.Parse("2-16-2015")},
                new PaidHoliday{holidayName="Memorial Day",
                holidayDate=DateTime.Parse("5-25-2015")},
                new PaidHoliday{holidayName="Independence Day",
                holidayDate=DateTime.Parse("7-4-2015")},
                new PaidHoliday{holidayName="Labor Day",
                holidayDate=DateTime.Parse("9-7-2015")},
                new PaidHoliday{holidayName="Columbus Day",
                holidayDate=DateTime.Parse("10-12-2015")},
                new PaidHoliday{holidayName="Veterans Day",
                holidayDate=DateTime.Parse("11-11-2015")},
                new PaidHoliday{holidayName="Thanksgiving",
                holidayDate=DateTime.Parse("11-26-2015")},
                new PaidHoliday{holidayName="Christmas",
                holidayDate=DateTime.Parse("12-25-2015")}
            };

            holidays.ForEach(h => context.paidHolidays.AddOrUpdate(p => p.holidayName, h));
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

            outOfOfficeReasons.ForEach(o => context.outOfOfficeReasons.AddOrUpdate(p => p.reason, o));
            context.SaveChanges();

            var experienceLevel = new List<ExperienceLevel>
            {
                new ExperienceLevel{levelName="Senior"},
                new ExperienceLevel{levelName="Mid"},
                new ExperienceLevel{levelName="Junior"},
                new ExperienceLevel{levelName="New"}
            };

            experienceLevel.ForEach(e => context.experienceLevel.AddOrUpdate(p => p.levelName, e));
            context.SaveChanges();

            var applications = new List<Application>
            {
                new Application{appName="OLB", hasOnCall=true, rotationLength=1, hasSecondary=true},
                new Application{appName="MGR", hasOnCall=true, rotationLength=2, hasSecondary=false},
                new Application{appName="SM", hasOnCall=false, rotationLength=0, hasSecondary=false},
                new Application{appName="HSF", hasOnCall=true, rotationLength=2, hasSecondary=true}
            };

            applications.ForEach(a => context.applications.AddOrUpdate(p => p.appName, a));            
            context.SaveChanges();
        }
    }
}
