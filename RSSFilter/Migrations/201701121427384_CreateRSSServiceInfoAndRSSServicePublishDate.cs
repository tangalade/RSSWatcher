namespace RSSFilter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateRSSServiceInfoAndRSSServicePublishDate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RSSServiceInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JobId = c.String(nullable: false),
                        Name = c.String(),
                        IsRunning = c.Boolean(nullable: false),
                        CreationDate = c.DateTimeOffset(nullable: false, precision: 7),
                        LastStartDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.RSSItems", "PublishDate", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RSSItems", "PublishDate");
            DropTable("dbo.RSSServiceInfoes");
        }
    }
}
