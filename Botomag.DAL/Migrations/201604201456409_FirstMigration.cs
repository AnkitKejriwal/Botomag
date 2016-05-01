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
                "dbo.BotStats",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Requests = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bots", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Commands",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        BotId = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        CurrentState = c.Int(nullable: false),
                        NextState = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bots", t => t.BotId, cascadeDelete: true)
                .Index(t => t.BotId);
            
            CreateTable(
                "dbo.Responses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Text = c.String(nullable: false),
                        ParseMode = c.Int(),
                        Keyboard = c.String(),
                        Command_Id = c.Guid(),
                        Bot_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Commands", t => t.Command_Id)
                .ForeignKey("dbo.Bots", t => t.Bot_Id)
                .Index(t => t.Command_Id)
                .Index(t => t.Bot_Id);
            
            CreateTable(
                "dbo.Parameters",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Expression = c.String(),
                        Type = c.Int(nullable: false),
                        CommandId = c.Guid(nullable: false),
                        Response_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Responses", t => t.Response_Id)
                .ForeignKey("dbo.Commands", t => t.CommandId, cascadeDelete: true)
                .Index(t => t.CommandId)
                .Index(t => t.Response_Id);
            
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
            DropForeignKey("dbo.Responses", "Bot_Id", "dbo.Bots");
            DropForeignKey("dbo.Commands", "BotId", "dbo.Bots");
            DropForeignKey("dbo.Parameters", "CommandId", "dbo.Commands");
            DropForeignKey("dbo.Parameters", "Response_Id", "dbo.Responses");
            DropForeignKey("dbo.Responses", "Command_Id", "dbo.Commands");
            DropForeignKey("dbo.BotStats", "Id", "dbo.Bots");
            DropIndex("dbo.LastUpdates", new[] { "BotId" });
            DropIndex("dbo.Parameters", new[] { "Response_Id" });
            DropIndex("dbo.Parameters", new[] { "CommandId" });
            DropIndex("dbo.Responses", new[] { "Bot_Id" });
            DropIndex("dbo.Responses", new[] { "Command_Id" });
            DropIndex("dbo.Commands", new[] { "BotId" });
            DropIndex("dbo.BotStats", new[] { "Id" });
            DropTable("dbo.LastUpdates");
            DropTable("dbo.Parameters");
            DropTable("dbo.Responses");
            DropTable("dbo.Commands");
            DropTable("dbo.BotStats");
            DropTable("dbo.Bots");
        }
    }
}
