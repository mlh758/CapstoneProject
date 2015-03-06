namespace On_Call_Assistant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Application",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        appPriority = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Employee",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        firstName = c.String(),
                        lastName = c.String(),
                        alottedVacationHours = c.Int(nullable: false),
                        _role = c.String(),
                        applicationID = c.Int(nullable: false),
                        OnCallRotation_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Application", t => t.applicationID, cascadeDelete: true)
                .ForeignKey("dbo.OnCallRotation", t => t.OnCallRotation_ID)
                .Index(t => t.applicationID)
                .Index(t => t.OnCallRotation_ID);
            
            CreateTable(
                "dbo.HasPaidHoliday",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        paidHolidayID = c.Int(nullable: false),
                        onCallRotationID = c.Int(nullable: false),
                        holidayDate = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OnCallRotation", t => t.onCallRotationID, cascadeDelete: true)
                .Index(t => t.onCallRotationID);
            
            CreateTable(
                "dbo.PaidHoliday",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        HasPaidHoliday_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.HasPaidHoliday", t => t.HasPaidHoliday_ID)
                .Index(t => t.HasPaidHoliday_ID);
            
            CreateTable(
                "dbo.OnCallRotation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        startDate = c.String(),
                        endDate = c.String(),
                        isPrimatry = c.Boolean(nullable: false),
                        employeeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.IsOnRotation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        employeeID = c.Int(nullable: false),
                        rotationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Employee", t => t.employeeID, cascadeDelete: true)
                .ForeignKey("dbo.OnCallRotation", t => t.rotationID, cascadeDelete: true)
                .Index(t => t.employeeID)
                .Index(t => t.rotationID);
            
            CreateTable(
                "dbo.IsOutOfOffice",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        employeeID = c.Int(nullable: false),
                        outOfOfficeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Employee", t => t.employeeID, cascadeDelete: true)
                .ForeignKey("dbo.OutOfOffice", t => t.outOfOfficeID, cascadeDelete: true)
                .Index(t => t.employeeID)
                .Index(t => t.outOfOfficeID);
            
            CreateTable(
                "dbo.OutOfOffice",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        numHours = c.Int(nullable: false),
                        _date = c.String(),
                        outOfOfficeReasonID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OutOfOfficeReason", t => t.outOfOfficeReasonID, cascadeDelete: true)
                .Index(t => t.outOfOfficeReasonID);
            
            CreateTable(
                "dbo.OutOfOfficeReason",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        reason = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IsOutOfOffice", "outOfOfficeID", "dbo.OutOfOffice");
            DropForeignKey("dbo.OutOfOffice", "outOfOfficeReasonID", "dbo.OutOfOfficeReason");
            DropForeignKey("dbo.IsOutOfOffice", "employeeID", "dbo.Employee");
            DropForeignKey("dbo.IsOnRotation", "rotationID", "dbo.OnCallRotation");
            DropForeignKey("dbo.IsOnRotation", "employeeID", "dbo.Employee");
            DropForeignKey("dbo.HasPaidHoliday", "onCallRotationID", "dbo.OnCallRotation");
            DropForeignKey("dbo.Employee", "OnCallRotation_ID", "dbo.OnCallRotation");
            DropForeignKey("dbo.PaidHoliday", "HasPaidHoliday_ID", "dbo.HasPaidHoliday");
            DropForeignKey("dbo.Employee", "applicationID", "dbo.Application");
            DropIndex("dbo.OutOfOffice", new[] { "outOfOfficeReasonID" });
            DropIndex("dbo.IsOutOfOffice", new[] { "outOfOfficeID" });
            DropIndex("dbo.IsOutOfOffice", new[] { "employeeID" });
            DropIndex("dbo.IsOnRotation", new[] { "rotationID" });
            DropIndex("dbo.IsOnRotation", new[] { "employeeID" });
            DropIndex("dbo.PaidHoliday", new[] { "HasPaidHoliday_ID" });
            DropIndex("dbo.HasPaidHoliday", new[] { "onCallRotationID" });
            DropIndex("dbo.Employee", new[] { "OnCallRotation_ID" });
            DropIndex("dbo.Employee", new[] { "applicationID" });
            DropTable("dbo.OutOfOfficeReason");
            DropTable("dbo.OutOfOffice");
            DropTable("dbo.IsOutOfOffice");
            DropTable("dbo.IsOnRotation");
            DropTable("dbo.OnCallRotation");
            DropTable("dbo.PaidHoliday");
            DropTable("dbo.HasPaidHoliday");
            DropTable("dbo.Employee");
            DropTable("dbo.Application");
        }
    }
}
