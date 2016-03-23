namespace Botomag.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BetTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Bots",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        LastUpdate = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Fights",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Date = c.DateTime(nullable: false),
                        OrganizationId = c.Guid(nullable: false),
                        BetTypeId = c.Guid(nullable: false),
                        Bet = c.String(nullable: false),
                        Factor = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BetTypes", t => t.BetTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Organizations", t => t.OrganizationId, cascadeDelete: true)
                .Index(t => t.OrganizationId)
                .Index(t => t.BetTypeId);
            
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Fights", "OrganizationId", "dbo.Organizations");
            DropForeignKey("dbo.Fights", "BetTypeId", "dbo.BetTypes");
            DropIndex("dbo.Fights", new[] { "BetTypeId" });
            DropIndex("dbo.Fights", new[] { "OrganizationId" });
            DropTable("dbo.Organizations");
            DropTable("dbo.Fights");
            DropTable("dbo.Bots");
            DropTable("dbo.BetTypes");
        }
    }
}
