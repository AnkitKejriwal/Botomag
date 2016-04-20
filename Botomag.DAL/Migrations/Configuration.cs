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
            Guid testBotId = new Guid("9EFD5204-F97A-4AC2-8163-7EFF4B73E2F2");
            string testBotToken = "219824686:AAFkrCCS1yRhdjKBfUrNbd6VlzNiDChHOMc";
            Bot testBot = context.Bots.Where(n => n.Id == testBotId).FirstOrDefault();
            if (testBot == null)
            {
                testBot = new Bot
                {
                    Id = testBotId,
                    Token = testBotToken,
                    BotStat = new BotStat { Id = testBotId, Requests = 0 }
                };

                string commandexp = "/factor";
                Command command = new Command 
                {
                    Id = Guid.NewGuid(),
                    BotId = testBot.Id, 
                    Name = commandexp,
                    CurrentState = 0,
                    NextState = 0,     
                };


                Response response1 = new Response 
                {
                    Id = Guid.NewGuid(), 
                    Text = "Дата: 10.04.2016 21:00 Организация: UFC Тип ставки: Ординар Ставка: 1,3 Бой:" +
                    "Николас Дэлби - Зак Каммингс. П1." 
                };
                // 1,00-1,42
                string paramexp1 = @"^1(?([,.])[,.](?([0-3])[0-3]\d?|4[0-2]?))$";
                Parameter param1 = new Parameter
                {
                    Id = Guid.NewGuid(),
                    CommandId = command.Id,
                    Expression = paramexp1,
                    Type = ParameterTypes.RegularExpression,
                    Response = response1,
                     
                };

                Response response2 = new Response
                {
                    Id = Guid.NewGuid(),
                    Text = "Дата: 10.04.2016 21:00 Организация: UFC Тип ставки: Ординар Ставка: 1,55 Бой: " + 
                    "Габриэль Гонзага - Деррик Льюис. П2."
                };

                // 1,43-1,57
                string paramexp2 = @"^1[,.](?([4])[4][3-9]|[5][0-7]?)$";
                Parameter param2 = new Parameter
                {
                    Id = Guid.NewGuid(),
                    CommandId = command.Id,
                    Expression = paramexp2,
                    Type = ParameterTypes.RegularExpression,
                    Response = response2,

                };

                Response response3 = new Response
                {
                    Id = Guid.NewGuid(),
                    Text = "Дата: 10.04.2016 21:00 Организация: UFC Тип ставки: Ординар Ставка: 1,55 Бой: " + 
                    "Габриэль Гонзага - Деррик Льюис. П2."
                };

                // 1,58-100,00
                string paramexp3 = @"^(?([1])[1](?([,.])[.,](?([5])[5][8-9]|[6-9]\d?)|(?([0])[0]{1,2}|[1-9]))|[2-9](?([,.])[,.]\d{1,2}|\d?))$";
                Parameter param3 = new Parameter
                {
                    Id = Guid.NewGuid(),
                    CommandId = command.Id,
                    Expression = paramexp3,
                    Type = ParameterTypes.RegularExpression,
                    Response = response3,

                };

                command.Parameters = new HashSet<Parameter>
                {
                    param1, param2, param3
                };

                testBot.Commands = new HashSet<Command>
                {
                    command
                };

                context.Bots.Add(testBot);
                context.SaveChanges();
            }
        }
    }
}
