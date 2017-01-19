namespace RSSFilter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRSSItemFields : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Artists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ItemTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.RSSItems", "RSSTitle", c => c.String(nullable: false));
            AddColumn("dbo.RSSItems", "CorrectlyParsed", c => c.Boolean(nullable: false));
            AddColumn("dbo.RSSItems", "ItemTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.RSSItems", "ArtistId", c => c.Int(nullable: false));
            AddColumn("dbo.RSSItems", "Title", c => c.String());
            AlterColumn("dbo.RSSItems", "RSSId", c => c.String(nullable: false, maxLength: 255));
            CreateIndex("dbo.RSSItems", "ItemTypeId");
            CreateIndex("dbo.RSSItems", "ArtistId");
            AddForeignKey("dbo.RSSItems", "ArtistId", "dbo.Artists", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RSSItems", "ItemTypeId", "dbo.ItemTypes", "Id", cascadeDelete: true);
            DropColumn("dbo.RSSItems", "RSSData");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RSSItems", "RSSData", c => c.String());
            DropForeignKey("dbo.RSSItems", "ItemTypeId", "dbo.ItemTypes");
            DropForeignKey("dbo.RSSItems", "ArtistId", "dbo.Artists");
            DropIndex("dbo.RSSItems", new[] { "ArtistId" });
            DropIndex("dbo.RSSItems", new[] { "ItemTypeId" });
            AlterColumn("dbo.RSSItems", "RSSId", c => c.Int(nullable: false));
            DropColumn("dbo.RSSItems", "Title");
            DropColumn("dbo.RSSItems", "ArtistId");
            DropColumn("dbo.RSSItems", "ItemTypeId");
            DropColumn("dbo.RSSItems", "CorrectlyParsed");
            DropColumn("dbo.RSSItems", "RSSTitle");
            DropTable("dbo.ItemTypes");
            DropTable("dbo.Artists");
        }
    }
}
