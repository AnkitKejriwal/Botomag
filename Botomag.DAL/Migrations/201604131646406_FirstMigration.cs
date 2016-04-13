namespace Botomag.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bots",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Token = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Commands",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        BotId = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        CommandType = c.Int(nullable: false),
                        CurrentState = c.Int(nullable: false),
                        NextState = c.Int(nullable: false),
                        ResponseId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Responses", t => t.Id)
                .ForeignKey("dbo.Bots", t => t.BotId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.BotId);
            
            CreateTable(
                "dbo.Responses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Text = c.String(nullable: false),
                        ParseMode = c.Int(),
                        Keyboard = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LastUpdates",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ChatId = c.Int(nullable: false),
                        UpdateId = c.Int(nullable: false),
                        BotId = c.Guid(nullable: false),
                        CurrentState = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bots", t => t.BotId, cascadeDelete: true)
                .Index(t => t.BotId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LastUpdates", "BotId", "dbo.Bots");
            DropForeignKey("dbo.Commands", "BotId", "dbo.Bots");
            DropForeignKey("dbo.Commands", "Id", "dbo.Responses");
            DropIndex("dbo.LastUpdates", new[] { "BotId" });
            DropIndex("dbo.Commands", new[] { "BotId" });
            DropIndex("dbo.Commands", new[] { "Id" });
            DropTable("dbo.LastUpdates");
            DropTable("dbo.Responses");
            DropTable("dbo.Commands");
            DropTable("dbo.Bots");
        }
    }
}
