namespace On_Call_Assistant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HasPaidHolidayLookUpTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PaidHolidayOnCallRotation", newName: "HasPaidHoliday");
            RenameColumn(table: "dbo.HasPaidHoliday", name: "PaidHoliday_paidHolidayID", newName: "paidHolidayID");
            RenameColumn(table: "dbo.HasPaidHoliday", name: "OnCallRotation_rotationID", newName: "rotationID");
            RenameIndex(table: "dbo.HasPaidHoliday", name: "IX_OnCallRotation_rotationID", newName: "IX_rotationID");
            RenameIndex(table: "dbo.HasPaidHoliday", name: "IX_PaidHoliday_paidHolidayID", newName: "IX_paidHolidayID");
            DropPrimaryKey("dbo.HasPaidHoliday");
            AddPrimaryKey("dbo.HasPaidHoliday", new[] { "rotationID", "paidHolidayID" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.HasPaidHoliday");
            AddPrimaryKey("dbo.HasPaidHoliday", new[] { "PaidHoliday_paidHolidayID", "OnCallRotation_rotationID" });
            RenameIndex(table: "dbo.HasPaidHoliday", name: "IX_paidHolidayID", newName: "IX_PaidHoliday_paidHolidayID");
            RenameIndex(table: "dbo.HasPaidHoliday", name: "IX_rotationID", newName: "IX_OnCallRotation_rotationID");
            RenameColumn(table: "dbo.HasPaidHoliday", name: "rotationID", newName: "OnCallRotation_rotationID");
            RenameColumn(table: "dbo.HasPaidHoliday", name: "paidHolidayID", newName: "PaidHoliday_paidHolidayID");
            RenameTable(name: "dbo.HasPaidHoliday", newName: "PaidHolidayOnCallRotation");
        }
    }
}
