namespace Botomag.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTokenRemoveNameFromBot : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bots", "Token", c => c.String(nullable: false));
            DropColumn("dbo.Bots", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bots", "Name", c => c.String(nullable: false));
            DropColumn("dbo.Bots", "Token");
        }
    }
}
