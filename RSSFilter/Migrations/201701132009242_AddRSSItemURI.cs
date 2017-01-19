namespace RSSFilter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRSSItemURI : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RSSItems", "URI", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RSSItems", "URI");
        }
    }
}
