using System;
using System.Data.Entity.ModelConfiguration;

namespace Botomag.DAL.Model
{
    public class FightConfiguration : BaseConfiguration<Fight, Guid>
    {
        public FightConfiguration()
        {
            HasRequired(n => n.Organization).WithMany().HasForeignKey(n => n.OrganizationId);
            HasRequired(n => n.BetType).WithMany().HasForeignKey(n => n.BetTypeId);
            Property(n => n.Bet).IsRequired();
        }
    }
}
