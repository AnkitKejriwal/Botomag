using System;
using System.Data.Entity;
using Botomag.DAL.Model;

namespace Botomag.DAL
{
    /// <summary>
    /// Implements context for app
    /// </summary>
    public class Context :  DbContext
    {
        public Context() : base("Name=FightBetBotConnectionString") { }

        public DbSet<BetType> BetTypes { get; set; }

        public DbSet<Bot> Bots { get; set;}

        public DbSet<Fight> Fights { get; set; }

        public DbSet<Organization> Organizations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new BetTypeConfiguration());
            modelBuilder.Configurations.Add(new BotConfiguration());
            modelBuilder.Configurations.Add(new FightConfiguration());
            modelBuilder.Configurations.Add(new OrganizationConfiguration());

        }
    }
}
