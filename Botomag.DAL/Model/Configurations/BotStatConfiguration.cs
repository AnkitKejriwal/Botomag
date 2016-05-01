using System;
using System.Data.Entity.ModelConfiguration;

namespace Botomag.DAL.Model.Configurations
{
    public class BotStatConfiguration : BaseConfiguration<BotStat, Guid>
    {
        public BotStatConfiguration()
        {
            HasRequired(n => n.Bot).WithRequiredDependent(n => n.BotStat);
        }
    }
}
