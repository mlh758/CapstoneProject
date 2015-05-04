using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using On_Call_Assistant.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace On_Call_Assistant.DAL
{
    public class OnCallContext : DbContext
    {
        public OnCallContext() : base("OnCallContext")
        {
        }

        public DbSet<Application> applications { get; set; }
        public DbSet<Employee> employees { get; set; }
        public DbSet<OnCallRotation> onCallRotations { get; set; }
        public DbSet<OutOfOffice> outOfOffice { get; set; }
        public DbSet<OutOfOfficeReason> outOfOfficeReasons { get; set; }
        public DbSet<PaidHoliday> paidHolidays { get; set; }
        public DbSet<ExperienceLevel> experienceLevel { get; set; }

        /** Called when the model is initially created. Also gets called anytime
         *  the add-migration command is used from the PM Console, only if there
         *  is change within the method.
         **/
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<OnCallRotation>().HasMany(r => r.holidays).WithMany(h => h.rotations)
            .Map(t => t.MapLeftKey("rotationID")
            .MapRightKey("paidHolidayID")
            .ToTable("HasPaidHoliday"));
        }
    }
}