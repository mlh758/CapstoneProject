namespace On_Call_Assistant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HasPaidHolidayLookUpTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PaidHoliday", new[] { "HasPaidHoliday_onCallRotationID", "HasPaidHoliday_paidHolidayID" }, "dbo.HasPaidHoliday");
            DropForeignKey("dbo.HasPaidHoliday", "onCallRotationID", "dbo.OnCallRotation");
            DropIndex("dbo.HasPaidHoliday", new[] { "onCallRotationID" });
            DropIndex("dbo.PaidHoliday", new[] { "HasPaidHoliday_onCallRotationID", "HasPaidHoliday_paidHolidayID" });
            CreateTable(
                "dbo.HasPaidHoliday",
                c => new
                    {
                        rotationID = c.Int(nullable: false),
                        paidHolidayID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.rotationID, t.paidHolidayID })
                .ForeignKey("dbo.OnCallRotation", t => t.rotationID, cascadeDelete: true)
                .ForeignKey("dbo.PaidHoliday", t => t.paidHolidayID, cascadeDelete: true)
                .Index(t => t.rotationID)
                .Index(t => t.paidHolidayID);
            
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
            DropForeignKey("dbo.HasPaidHoliday", "paidHolidayID", "dbo.PaidHoliday");
            DropForeignKey("dbo.HasPaidHoliday", "rotationID", "dbo.OnCallRotation");
            DropIndex("dbo.HasPaidHoliday", new[] { "paidHolidayID" });
            DropIndex("dbo.HasPaidHoliday", new[] { "rotationID" });
            DropTable("dbo.HasPaidHoliday");
            CreateIndex("dbo.PaidHoliday", new[] { "HasPaidHoliday_onCallRotationID", "HasPaidHoliday_paidHolidayID" });
            CreateIndex("dbo.HasPaidHoliday", "onCallRotationID");
            AddForeignKey("dbo.HasPaidHoliday", "onCallRotationID", "dbo.OnCallRotation", "ID", cascadeDelete: true);
            AddForeignKey("dbo.PaidHoliday", new[] { "HasPaidHoliday_onCallRotationID", "HasPaidHoliday_paidHolidayID" }, "dbo.HasPaidHoliday", new[] { "onCallRotationID", "paidHolidayID" });
        }
    }
}
