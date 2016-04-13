namespace Botomag.DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Botomag.DAL.Model;
    using System.Collections.Generic;

    public sealed class Configuration : DbMigrationsConfiguration<Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Context context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            //create entries for test bot
            Bot testBot = context.Bots.Where(n => n.Token == "219824686:AAFkrCCS1yRhdjKBfUrNbd6VlzNiDChHOMc").FirstOrDefault();
            if (testBot == null)
            {
                testBot = new Bot
                {
                    Id = Guid.NewGuid(),
                    Token = "219824686:AAFkrCCS1yRhdjKBfUrNbd6VlzNiDChHOMc",
                };
                Response response = new Response { Id = Guid.NewGuid(), Text = "Success!" };
                testBot.Commands = new HashSet<Command>
                {
                    new Command { 
                        BotId = testBot.Id, 
                        CommandType = CommandTypes.Literal, 
                        CurrentState = 0, 
                        Name = "/test", 
                        Response = response,
                        ResponseId = response.Id,
                        NextState = 0, 
                        Id = Guid.NewGuid() 
                    }
                };
                context.Bots.Add(testBot);
                context.SaveChanges();
            }
        }
    }
}
