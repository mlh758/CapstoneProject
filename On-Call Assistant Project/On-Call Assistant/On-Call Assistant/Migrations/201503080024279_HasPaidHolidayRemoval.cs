namespace On_Call_Assistant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HasPaidHolidayRemoval : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PaidHoliday", new[] { "HasPaidHoliday_onCallRotationID", "HasPaidHoliday_paidHolidayID" }, "dbo.HasPaidHoliday");
            DropForeignKey("dbo.HasPaidHoliday", "onCallRotationID", "dbo.OnCallRotation");
            DropIndex("dbo.HasPaidHoliday", new[] { "onCallRotationID" });
            DropIndex("dbo.PaidHoliday", new[] { "HasPaidHoliday_onCallRotationID", "HasPaidHoliday_paidHolidayID" });
            CreateTable(
                "dbo.PaidHolidayOnCallRotation",
                c => new
                    {
                        PaidHoliday_paidHolidayID = c.Int(nullable: false),
                        OnCallRotation_rotationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PaidHoliday_paidHolidayID, t.OnCallRotation_rotationID })
                .ForeignKey("dbo.PaidHoliday", t => t.PaidHoliday_paidHolidayID, cascadeDelete: true)
                .ForeignKey("dbo.OnCallRotation", t => t.OnCallRotation_rotationID, cascadeDelete: true)
                .Index(t => t.PaidHoliday_paidHolidayID)
                .Index(t => t.OnCallRotation_rotationID);
            
            DropColumn("dbo.PaidHoliday", "HasPaidHoliday_onCallRotationID");
            DropColumn("dbo.PaidHoliday", "HasPaidHoliday_paidHolidayID");
            DropTable("dbo.HasPaidHoliday");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.HasPaidHoliday",
                c => new
                    {
                        onCallRotationID = c.Int(nullable: false),
                        paidHolidayID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.onCallRotationID, t.paidHolidayID });
            
            AddColumn("dbo.PaidHoliday", "HasPaidHoliday_paidHolidayID", c => c.Int());
            AddColumn("dbo.PaidHoliday", "HasPaidHoliday_onCallRotationID", c => c.Int());
            DropForeignKey("dbo.PaidHolidayOnCallRotation", "OnCallRotation_rotationID", "dbo.OnCallRotation");
            DropForeignKey("dbo.PaidHolidayOnCallRotation", "PaidHoliday_paidHolidayID", "dbo.PaidHoliday");
            DropIndex("dbo.PaidHolidayOnCallRotation", new[] { "OnCallRotation_rotationID" });
            DropIndex("dbo.PaidHolidayOnCallRotation", new[] { "PaidHoliday_paidHolidayID" });
            DropTable("dbo.PaidHolidayOnCallRotation");
            CreateIndex("dbo.PaidHoliday", new[] { "HasPaidHoliday_onCallRotationID", "HasPaidHoliday_paidHolidayID" });
            CreateIndex("dbo.HasPaidHoliday", "onCallRotationID");
            AddForeignKey("dbo.HasPaidHoliday", "onCallRotationID", "dbo.OnCallRotation", "ID", cascadeDelete: true);
            AddForeignKey("dbo.PaidHoliday", new[] { "HasPaidHoliday_onCallRotationID", "HasPaidHoliday_paidHolidayID" }, "dbo.HasPaidHoliday", new[] { "onCallRotationID", "paidHolidayID" });
        }
    }
}
