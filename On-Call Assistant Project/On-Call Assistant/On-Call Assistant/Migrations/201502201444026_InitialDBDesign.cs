namespace On_Call_Assistant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDBDesign : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExperienceLevel",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        level = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Application", "appName", c => c.String());
            AddColumn("dbo.Application", "rotationLength", c => c.Int(nullable: false));
            AddColumn("dbo.Application", "hasOnCall", c => c.Boolean(nullable: false));
            AddColumn("dbo.Employee", "email", c => c.String());
            AddColumn("dbo.Employee", "hiredDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Employee", "birthday", c => c.DateTime(nullable: false));
            AddColumn("dbo.Employee", "experienceID", c => c.Int(nullable: false));
            AddColumn("dbo.Employee", "experienceLevel_ID", c => c.Int());
            AddColumn("dbo.PaidHoliday", "holidayName", c => c.String());
            AddColumn("dbo.OutOfOffice", "startDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.HasPaidHoliday", "holidayDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.OnCallRotation", "startDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.OnCallRotation", "endDate", c => c.DateTime(nullable: false));
            CreateIndex("dbo.Employee", "experienceLevel_ID");
            AddForeignKey("dbo.Employee", "experienceLevel_ID", "dbo.ExperienceLevel", "ID");
            DropColumn("dbo.Application", "name");
            DropColumn("dbo.Application", "appPriority");
            DropColumn("dbo.Employee", "_role");
            DropColumn("dbo.PaidHoliday", "name");
            DropColumn("dbo.OutOfOffice", "_date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OutOfOffice", "_date", c => c.String());
            AddColumn("dbo.PaidHoliday", "name", c => c.String());
            AddColumn("dbo.Employee", "_role", c => c.String());
            AddColumn("dbo.Application", "appPriority", c => c.Int(nullable: false));
            AddColumn("dbo.Application", "name", c => c.String());
            DropForeignKey("dbo.Employee", "experienceLevel_ID", "dbo.ExperienceLevel");
            DropIndex("dbo.Employee", new[] { "experienceLevel_ID" });
            AlterColumn("dbo.OnCallRotation", "endDate", c => c.String());
            AlterColumn("dbo.OnCallRotation", "startDate", c => c.String());
            AlterColumn("dbo.HasPaidHoliday", "holidayDate", c => c.String());
            DropColumn("dbo.OutOfOffice", "startDate");
            DropColumn("dbo.PaidHoliday", "holidayName");
            DropColumn("dbo.Employee", "experienceLevel_ID");
            DropColumn("dbo.Employee", "experienceID");
            DropColumn("dbo.Employee", "birthday");
            DropColumn("dbo.Employee", "hiredDate");
            DropColumn("dbo.Employee", "email");
            DropColumn("dbo.Application", "hasOnCall");
            DropColumn("dbo.Application", "rotationLength");
            DropColumn("dbo.Application", "appName");
            DropTable("dbo.ExperienceLevel");
        }
    }
}
