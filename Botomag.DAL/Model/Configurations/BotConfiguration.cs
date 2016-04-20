using System;
using System.Data.Entity.ModelConfiguration;

namespace Botomag.DAL.Model.Configurations
{
    public class BotConfiguration : BaseConfiguration<Bot, Guid>
    {
        public BotConfiguration()
        {
            Property(n => n.Token).IsRequired();
            HasMany(n => n.Commands).WithRequired(n => n.Bot).HasForeignKey(n => n.BotId);
            HasMany(n => n.LastUpdates).WithRequired(n => n.Bot).HasForeignKey(n => n.BotId);
            HasRequired(n => n.BotStat).WithRequiredPrincipal(n => n.Bot);
            HasOptional(n => n.InvalidCommandResponse).WithOptionalPrincipal();
        }
    }
}
