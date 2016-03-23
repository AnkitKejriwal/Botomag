using System;
using System.Data.Entity.ModelConfiguration;

namespace Botomag.DAL.Model
{
    public class OrganizationConfiguration : BaseConfiguration<Organization, Guid>
    {
        public OrganizationConfiguration()
        {
            Property(n => n.Title).IsRequired();
        }
    }
}
