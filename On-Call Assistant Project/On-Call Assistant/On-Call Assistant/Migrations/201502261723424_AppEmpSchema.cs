namespace On_Call_Assistant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppEmpSchema : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Application", "appName", c => c.String(nullable: false, maxLength: 15));
            AlterColumn("dbo.Employee", "firstName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Employee", "lastName", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employee", "lastName", c => c.String());
            AlterColumn("dbo.Employee", "firstName", c => c.String());
            AlterColumn("dbo.Application", "appName", c => c.String(maxLength: 15));
        }
    }
}
