namespace On_Call_Assistant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppSecDisplayColor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Application", "primDisplayColor", c => c.String());
            AddColumn("dbo.Application", "secDisplayColor", c => c.String());
            DropColumn("dbo.Application", "displayColor");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Application", "displayColor", c => c.String());
            DropColumn("dbo.Application", "secDisplayColor");
            DropColumn("dbo.Application", "primDisplayColor");
        }
    }
}
