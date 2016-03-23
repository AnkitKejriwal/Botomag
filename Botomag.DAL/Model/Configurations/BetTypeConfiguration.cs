using System;
using System.Data.Entity.ModelConfiguration;

namespace Botomag.DAL.Model
{
    public class BetTypeConfiguration : BaseConfiguration<BetType, Guid>
    {
        public BetTypeConfiguration()
        {
            Property(n => n.Title).IsRequired();
        }
    }
}
