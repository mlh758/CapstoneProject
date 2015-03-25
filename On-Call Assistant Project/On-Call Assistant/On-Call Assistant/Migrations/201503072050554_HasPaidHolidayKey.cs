namespace On_Call_Assistant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HasPaidHolidayKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PaidHoliday", "HasPaidHoliday_ID", "dbo.HasPaidHoliday");
            DropIndex("dbo.PaidHoliday", new[] { "HasPaidHoliday_ID" });
            RenameColumn(table: "dbo.PaidHoliday", name: "HasPaidHoliday_ID", newName: "HasPaidHoliday_onCallRotationID");
            DropPrimaryKey("dbo.HasPaidHoliday");
            AddColumn("dbo.PaidHoliday", "HasPaidHoliday_paidHolidayID", c => c.Int());
            AddPrimaryKey("dbo.HasPaidHoliday", new[] { "onCallRotationID", "paidHolidayID" });
            CreateIndex("dbo.PaidHoliday", new[] { "HasPaidHoliday_onCallRotationID", "HasPaidHoliday_paidHolidayID" });
            AddForeignKey("dbo.PaidHoliday", new[] { "HasPaidHoliday_onCallRotationID", "HasPaidHoliday_paidHolidayID" }, "dbo.HasPaidHoliday", new[] { "onCallRotationID", "paidHolidayID" });
            DropColumn("dbo.HasPaidHoliday", "ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HasPaidHoliday", "ID", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.PaidHoliday", new[] { "HasPaidHoliday_onCallRotationID", "HasPaidHoliday_paidHolidayID" }, "dbo.HasPaidHoliday");
            DropIndex("dbo.PaidHoliday", new[] { "HasPaidHoliday_onCallRotationID", "HasPaidHoliday_paidHolidayID" });
            DropPrimaryKey("dbo.HasPaidHoliday");
            DropColumn("dbo.PaidHoliday", "HasPaidHoliday_paidHolidayID");
            AddPrimaryKey("dbo.HasPaidHoliday", "ID");
            RenameColumn(table: "dbo.PaidHoliday", name: "HasPaidHoliday_onCallRotationID", newName: "HasPaidHoliday_ID");
            CreateIndex("dbo.PaidHoliday", "HasPaidHoliday_ID");
            AddForeignKey("dbo.PaidHoliday", "HasPaidHoliday_ID", "dbo.HasPaidHoliday", "ID");
        }
    }
}
