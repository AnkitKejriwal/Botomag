namespace Botomag.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUser_BotsAddBot_User : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bots", "UserId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Bots", "UserId");
            AddForeignKey("dbo.Bots", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bots", "UserId", "dbo.Users");
            DropIndex("dbo.Bots", new[] { "UserId" });
            DropColumn("dbo.Bots", "UserId");
        }
    }
}
