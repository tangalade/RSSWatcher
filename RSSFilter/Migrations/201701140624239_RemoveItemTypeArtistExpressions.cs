namespace RSSFilter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveItemTypeArtistExpressions : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ArtistMatchExpressions", "ArtistId", "dbo.Artists");
            DropForeignKey("dbo.ItemTypeMatchExpressions", "ItemTypeId", "dbo.ItemTypes");
            DropForeignKey("dbo.RSSItems", "ArtistMatchExpressionId", "dbo.ArtistMatchExpressions");
            DropForeignKey("dbo.RSSItems", "ItemTypeMatchExpressionId", "dbo.ItemTypeMatchExpressions");
            DropIndex("dbo.ArtistMatchExpressions", new[] { "ArtistId" });
            DropIndex("dbo.ItemTypeMatchExpressions", new[] { "ItemTypeId" });
            DropIndex("dbo.RSSItems", new[] { "ItemTypeMatchExpressionId" });
            DropIndex("dbo.RSSItems", new[] { "ArtistMatchExpressionId" });
            AddColumn("dbo.RSSItems", "RSSPublishDate", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.RSSItems", "RSSURI", c => c.String());
            AddColumn("dbo.RSSItems", "ItemTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.RSSItems", "ArtistId", c => c.Int(nullable: false));
            CreateIndex("dbo.RSSItems", "ItemTypeId");
            CreateIndex("dbo.RSSItems", "ArtistId");
            AddForeignKey("dbo.RSSItems", "ArtistId", "dbo.Artists", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RSSItems", "ItemTypeId", "dbo.ItemTypes", "Id", cascadeDelete: true);
            DropColumn("dbo.RSSItems", "ItemTypeMatchExpressionId");
            DropColumn("dbo.RSSItems", "ArtistMatchExpressionId");
            DropColumn("dbo.RSSItems", "PublishDate");
            DropColumn("dbo.RSSItems", "URI");
            DropTable("dbo.ArtistMatchExpressions");
            DropTable("dbo.ItemTypeMatchExpressions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ItemTypeMatchExpressions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Expression = c.String(nullable: false, maxLength: 255),
                        ItemTypeId = c.Int(nullable: false),
                        Resolved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ArtistMatchExpressions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Expression = c.String(nullable: false, maxLength: 255),
                        ArtistId = c.Int(nullable: false),
                        Resolved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.RSSItems", "URI", c => c.String());
            AddColumn("dbo.RSSItems", "PublishDate", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.RSSItems", "ArtistMatchExpressionId", c => c.Int(nullable: false));
            AddColumn("dbo.RSSItems", "ItemTypeMatchExpressionId", c => c.Int(nullable: false));
            DropForeignKey("dbo.RSSItems", "ItemTypeId", "dbo.ItemTypes");
            DropForeignKey("dbo.RSSItems", "ArtistId", "dbo.Artists");
            DropIndex("dbo.RSSItems", new[] { "ArtistId" });
            DropIndex("dbo.RSSItems", new[] { "ItemTypeId" });
            DropColumn("dbo.RSSItems", "ArtistId");
            DropColumn("dbo.RSSItems", "ItemTypeId");
            DropColumn("dbo.RSSItems", "RSSURI");
            DropColumn("dbo.RSSItems", "RSSPublishDate");
            CreateIndex("dbo.RSSItems", "ArtistMatchExpressionId");
            CreateIndex("dbo.RSSItems", "ItemTypeMatchExpressionId");
            CreateIndex("dbo.ItemTypeMatchExpressions", "ItemTypeId");
            CreateIndex("dbo.ArtistMatchExpressions", "ArtistId");
            AddForeignKey("dbo.RSSItems", "ItemTypeMatchExpressionId", "dbo.ItemTypeMatchExpressions", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RSSItems", "ArtistMatchExpressionId", "dbo.ArtistMatchExpressions", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ItemTypeMatchExpressions", "ItemTypeId", "dbo.ItemTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ArtistMatchExpressions", "ArtistId", "dbo.Artists", "Id", cascadeDelete: true);
        }
    }
}
