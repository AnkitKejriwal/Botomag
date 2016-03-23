using System;
using System.Data.Entity;
using Botomag.DAL;
using Botomag.DAL.Migrations;

namespace Botomag.Web
{
    public class MigrationsConfig
    {
        // TODO: transfer config to static constructor in db and remove ref to DAL layer from Web
        public static void Configuration()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Configuration>());
        }
    }
}