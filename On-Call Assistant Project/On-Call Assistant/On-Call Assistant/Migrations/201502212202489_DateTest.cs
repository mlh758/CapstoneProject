namespace On_Call_Assistant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateTest : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PaidHoliday", "holidayName", c => c.String(maxLength: 50));
            AlterColumn("dbo.OutOfOfficeReason", "reason", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OutOfOfficeReason", "reason", c => c.String());
            AlterColumn("dbo.PaidHoliday", "holidayName", c => c.String());
        }
    }
}
