namespace RSSFilter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRSSServiceInfoLastStopDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RSSServiceInfoes", "LastStopDate", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RSSServiceInfoes", "LastStopDate");
        }
    }
}
