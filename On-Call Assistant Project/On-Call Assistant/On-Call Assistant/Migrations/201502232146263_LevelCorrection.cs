namespace On_Call_Assistant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LevelCorrection : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExperienceLevel", "levelName", c => c.String(maxLength: 25));
            DropColumn("dbo.ExperienceLevel", "level");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ExperienceLevel", "level", c => c.String(maxLength: 25));
            DropColumn("dbo.ExperienceLevel", "levelName");
        }
    }
}
