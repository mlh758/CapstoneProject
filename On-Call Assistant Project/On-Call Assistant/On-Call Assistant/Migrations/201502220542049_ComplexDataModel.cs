namespace On_Call_Assistant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComplexDataModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ExperienceLevel", "level", c => c.String(maxLength: 25));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ExperienceLevel", "level", c => c.String());
        }
    }
}
