namespace RSSFilter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRSSServiceInfoRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RSSServiceInfoes", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RSSServiceInfoes", "Name", c => c.String());
        }
    }
}
