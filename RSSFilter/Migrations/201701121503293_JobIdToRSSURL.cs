namespace RSSFilter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobIdToRSSURL : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RSSServiceInfoes", "RSSURL", c => c.String(nullable: false));
            DropColumn("dbo.RSSServiceInfoes", "JobId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RSSServiceInfoes", "JobId", c => c.String(nullable: false));
            DropColumn("dbo.RSSServiceInfoes", "RSSURL");
        }
    }
}
