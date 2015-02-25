namespace On_Call_Assistant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComplexImplementation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Employee", "experienceLevel_ID", "dbo.ExperienceLevel");
            DropIndex("dbo.Employee", new[] { "experienceLevel_ID" });
            RenameColumn(table: "dbo.Employee", name: "experienceLevel_ID", newName: "experienceLevelID");
            AddColumn("dbo.PaidHoliday", "holidayDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Application", "appName", c => c.String(maxLength: 25));
            AlterColumn("dbo.Employee", "experienceLevelID", c => c.Int(nullable: false));
            AlterColumn("dbo.ExperienceLevel", "levelName", c => c.String(maxLength: 20));
            CreateIndex("dbo.Employee", "experienceLevelID");
            AddForeignKey("dbo.Employee", "experienceLevelID", "dbo.ExperienceLevel", "ID", cascadeDelete: true);
            DropColumn("dbo.Employee", "experienceID");
            DropColumn("dbo.HasPaidHoliday", "holidayDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HasPaidHoliday", "holidayDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Employee", "experienceID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Employee", "experienceLevelID", "dbo.ExperienceLevel");
            DropIndex("dbo.Employee", new[] { "experienceLevelID" });
            AlterColumn("dbo.ExperienceLevel", "levelName", c => c.String(maxLength: 25));
            AlterColumn("dbo.Employee", "experienceLevelID", c => c.Int());
            AlterColumn("dbo.Application", "appName", c => c.String());
            DropColumn("dbo.PaidHoliday", "holidayDate");
            RenameColumn(table: "dbo.Employee", name: "experienceLevelID", newName: "experienceLevel_ID");
            CreateIndex("dbo.Employee", "experienceLevel_ID");
            AddForeignKey("dbo.Employee", "experienceLevel_ID", "dbo.ExperienceLevel", "ID");
        }
    }
}
