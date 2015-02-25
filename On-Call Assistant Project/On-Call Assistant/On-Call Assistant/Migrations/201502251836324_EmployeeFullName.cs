namespace On_Call_Assistant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeFullName : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Application", "appName", c => c.String(maxLength: 15));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Application", "appName", c => c.String(maxLength: 25));
        }
    }
}
