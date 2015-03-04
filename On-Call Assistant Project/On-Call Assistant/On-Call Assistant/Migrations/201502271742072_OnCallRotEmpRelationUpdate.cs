namespace On_Call_Assistant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OnCallRotEmpRelationUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Employee", "OnCallRotation_ID", "dbo.OnCallRotation");
            DropIndex("dbo.Employee", new[] { "OnCallRotation_ID" });
            CreateIndex("dbo.OnCallRotation", "employeeID");
            AddForeignKey("dbo.OnCallRotation", "employeeID", "dbo.Employee", "ID", cascadeDelete: true);
            DropColumn("dbo.Employee", "OnCallRotation_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employee", "OnCallRotation_ID", c => c.Int());
            DropForeignKey("dbo.OnCallRotation", "employeeID", "dbo.Employee");
            DropIndex("dbo.OnCallRotation", new[] { "employeeID" });
            CreateIndex("dbo.Employee", "OnCallRotation_ID");
            AddForeignKey("dbo.Employee", "OnCallRotation_ID", "dbo.OnCallRotation", "ID");
        }
    }
}
