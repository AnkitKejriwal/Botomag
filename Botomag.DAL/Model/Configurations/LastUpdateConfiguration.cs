using System;
using System.Data.Entity.ModelConfiguration;

namespace Botomag.DAL.Model.Configurations
{
    public class LastUpdateConfiguration : BaseConfiguration<LastUpdate, Guid>
    {
        public LastUpdateConfiguration()
        {
            HasRequired(n => n.Bot).WithMany(n => n.LastUpates).HasForeignKey(n => n.BotId);
        }
    }
}
