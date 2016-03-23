namespace Botomag.DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Botomag.DAL.Model;

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

            if (context.BetTypes.Count() == 0)
            {
                context.BetTypes.AddRange(
                    new BetType[]
                    {
                        new BetType { Id = Guid.NewGuid(), Title = "Ординар" },
                        new BetType { Id = Guid.NewGuid(), Title = "Экспресс" },
                        new BetType { Id = Guid.NewGuid(), Title = "Система" },
                        new BetType { Id = Guid.NewGuid(), Title = "Регуляр" }
                    });
            }

            if (context.Organizations.Count() == 0)
            {
                context.Organizations.AddRange(
                    new Organization[]
                    {
                        new Organization { Id = Guid.NewGuid(), Title = "UFC" },
                        new Organization { Id = Guid.NewGuid(), Title = "Bellator" },
                        new Organization { Id = Guid.NewGuid(), Title = "WSOF" },
                        new Organization { Id = Guid.NewGuid(), Title = "ONE" },
                        new Organization { Id = Guid.NewGuid(), Title = "InvictaFC" },
                    });
            }

            context.SaveChanges();

            if (context.Fights.Count() == 0)
            {
                context.Fights.AddRange(
                    new Fight[]
                {
                    new Fight
                    {
                        Id = Guid.NewGuid(),
                        Bet = "Роджерс-Паттисон П1",
                        BetType = context.BetTypes.Where(n => n.Title == "Система").Single(),
                        Date = DateTime.Now + TimeSpan.FromDays(10),
                        Factor = 2.00M,
                        Organization = context.Organizations.Where(n => n.Title == "UFC").Single()
                    },
                    new Fight
                    {
                        Id = Guid.NewGuid(),
                        Bet = "Джейсон-Марков ТБ 2",
                        BetType = context.BetTypes.Where(n => n.Title == "Система").Single(),
                        Date = DateTime.Now + TimeSpan.FromDays(10),
                        Factor = 2.15M,
                        Organization = context.Organizations.Where(n => n.Title == "UFC").Single()
                    }
                });
            }
        }
    }
}
