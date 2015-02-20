namespace On_Call_Assistant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDBDesign1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OnCallRotation", "isPrimary", c => c.Boolean(nullable: false));
            DropColumn("dbo.OnCallRotation", "isPrimatry");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OnCallRotation", "isPrimatry", c => c.Boolean(nullable: false));
            DropColumn("dbo.OnCallRotation", "isPrimary");
        }
    }
}
