using System;
using System.Data.Entity.ModelConfiguration;

namespace Botomag.DAL.Model
{
    public class BotConfiguration : BaseConfiguration<Bot, Guid>
    {
        public BotConfiguration()
        {
            Property(n => n.Token).IsRequired();
        }
    }
}
