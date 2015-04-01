namespace On_Call_Assistant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppColor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Application", "displayColor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Application", "displayColor");
        }
    }
}
