using System;
using System.Data.Entity;
using Botomag.DAL.Model;
using Botomag.DAL.Model.Configurations;

namespace Botomag.DAL
{
    /// <summary>
    /// Implements context for app
    /// </summary>
    public class Context :  DbContext
    {
        public Context() : base("Name=Botomag") { }

        public DbSet<Bot> Bots { get; set;}

        public DbSet<Command> Commands { get; set; }

        public DbSet<Response> Responses { get; set; }

        public DbSet<LastUpdate> LastUpdates { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new BotConfiguration());

            modelBuilder.Configurations.Add(new CommandConfiguration());

            modelBuilder.Configurations.Add(new ResponseConfiguration());

            modelBuilder.Configurations.Add(new LastUpdateConfiguration());
        }
    }
}
