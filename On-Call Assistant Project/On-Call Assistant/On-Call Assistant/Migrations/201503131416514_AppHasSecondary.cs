namespace On_Call_Assistant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppHasSecondary : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Application", "hasSecondary", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Application", "hasSecondary");
        }
    }
}
