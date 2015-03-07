namespace On_Call_Assistant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OutOfOfficeEmpFK : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OutOfOffice", "employeeID", c => c.Int(nullable: false));
            CreateIndex("dbo.OutOfOffice", "employeeID");
            AddForeignKey("dbo.OutOfOffice", "employeeID", "dbo.Employee", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OutOfOffice", "employeeID", "dbo.Employee");
            DropIndex("dbo.OutOfOffice", new[] { "employeeID" });
            DropColumn("dbo.OutOfOffice", "employeeID");
        }
    }
}
