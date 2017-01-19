namespace RSSFilter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RSSServiceInfoNameStringLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RSSServiceInfoes", "Name", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RSSServiceInfoes", "Name", c => c.String(nullable: false));
        }
    }
}
