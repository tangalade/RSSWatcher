namespace RSSFilter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRSSItemViewStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RSSItems", "viewStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RSSItems", "viewStatus");
        }
    }
}
