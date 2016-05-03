namespace Botomag.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBot_NameBot_LastUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bots", "Name", c => c.String());
            AddColumn("dbo.Bots", "LastUpdate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bots", "LastUpdate");
            DropColumn("dbo.Bots", "Name");
        }
    }
}
