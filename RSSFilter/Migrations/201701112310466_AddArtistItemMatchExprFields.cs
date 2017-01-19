namespace RSSFilter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddArtistItemMatchExprFields : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RSSItems", "ArtistId", "dbo.Artists");
            DropForeignKey("dbo.RSSItems", "ItemTypeId", "dbo.ItemTypes");
            DropIndex("dbo.RSSItems", new[] { "ItemTypeId" });
            DropIndex("dbo.RSSItems", new[] { "ArtistId" });
            CreateTable(
                "dbo.ArtistMatchExpressions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Expression = c.String(nullable: false, maxLength: 255),
                        ArtistId = c.Int(nullable: false),
                        Resolved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Artists", t => t.ArtistId, cascadeDelete: true)
                .Index(t => t.ArtistId);
            
            CreateTable(
                "dbo.ItemTypeMatchExpressions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Expression = c.String(nullable: false, maxLength: 255),
                        ItemTypeId = c.Int(nullable: false),
                        Resolved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ItemTypes", t => t.ItemTypeId, cascadeDelete: true)
                .Index(t => t.ItemTypeId);
            
            AddColumn("dbo.RSSItems", "ItemTypeMatchExpressionId", c => c.Int(nullable: false));
            AddColumn("dbo.RSSItems", "ArtistMatchExpressionId", c => c.Int(nullable: false));
            CreateIndex("dbo.RSSItems", "ItemTypeMatchExpressionId");
            CreateIndex("dbo.RSSItems", "ArtistMatchExpressionId");
            AddForeignKey("dbo.RSSItems", "ArtistMatchExpressionId", "dbo.ArtistMatchExpressions", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RSSItems", "ItemTypeMatchExpressionId", "dbo.ItemTypeMatchExpressions", "Id", cascadeDelete: true);
            DropColumn("dbo.RSSItems", "ItemTypeId");
            DropColumn("dbo.RSSItems", "ArtistId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RSSItems", "ArtistId", c => c.Int(nullable: false));
            AddColumn("dbo.RSSItems", "ItemTypeId", c => c.Int(nullable: false));
            DropForeignKey("dbo.RSSItems", "ItemTypeMatchExpressionId", "dbo.ItemTypeMatchExpressions");
            DropForeignKey("dbo.RSSItems", "ArtistMatchExpressionId", "dbo.ArtistMatchExpressions");
            DropForeignKey("dbo.ItemTypeMatchExpressions", "ItemTypeId", "dbo.ItemTypes");
            DropForeignKey("dbo.ArtistMatchExpressions", "ArtistId", "dbo.Artists");
            DropIndex("dbo.RSSItems", new[] { "ArtistMatchExpressionId" });
            DropIndex("dbo.RSSItems", new[] { "ItemTypeMatchExpressionId" });
            DropIndex("dbo.ItemTypeMatchExpressions", new[] { "ItemTypeId" });
            DropIndex("dbo.ArtistMatchExpressions", new[] { "ArtistId" });
            DropColumn("dbo.RSSItems", "ArtistMatchExpressionId");
            DropColumn("dbo.RSSItems", "ItemTypeMatchExpressionId");
            DropTable("dbo.ItemTypeMatchExpressions");
            DropTable("dbo.ArtistMatchExpressions");
            CreateIndex("dbo.RSSItems", "ArtistId");
            CreateIndex("dbo.RSSItems", "ItemTypeId");
            AddForeignKey("dbo.RSSItems", "ItemTypeId", "dbo.ItemTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RSSItems", "ArtistId", "dbo.Artists", "Id", cascadeDelete: true);
        }
    }
}
