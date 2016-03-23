using System;
using System.Data.Entity;
using Botomag.DAL;
using Botomag.DAL.Migrations;
using System.Web.Mvc;

namespace Botomag.Web
{
    public class DbConfig
    {
        public static void Configuration()
        {
            Database.SetInitializer<Context>(new MigrateDatabaseToLatestVersion<Context, Configuration>());
        }
    }
}