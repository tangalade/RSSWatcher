namespace RSSFilter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddArtistItemTypeRating : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Artists", "Rating", c => c.Int(nullable: false));
            AddColumn("dbo.ItemTypes", "Rating", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ItemTypes", "Rating");
            DropColumn("dbo.Artists", "Rating");
        }
    }
}
