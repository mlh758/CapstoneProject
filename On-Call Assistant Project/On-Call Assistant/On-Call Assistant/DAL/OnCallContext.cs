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
        public DbSet<HasPaidHoliday> hasHolidays { get; set; }
        public DbSet<OnCallRotation> onCallRotations { get; set; }
        public DbSet<OutOfOffice> outOfOffice { get; set; }
        public DbSet<OutOfOfficeReason> outOfOfficeReasons { get; set; }
        public DbSet<PaidHoliday> paidHolidays { get; set; }
        //public DbSet<IsOnRotation> isOnRotation { get; set; }
        //public DbSet<HasReason> hasReason { get; set; }
        //public DbSet<IsOutOfOffice> isOutOfOffice { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}