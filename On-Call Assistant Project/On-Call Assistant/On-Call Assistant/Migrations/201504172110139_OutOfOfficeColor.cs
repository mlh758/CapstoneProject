namespace On_Call_Assistant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OutOfOfficeColor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OutOfOfficeReason", "reasonDisplayColor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OutOfOfficeReason", "reasonDisplayColor");
        }
    }
}
