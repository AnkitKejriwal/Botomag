using System;
using System.Data.Entity.ModelConfiguration;

namespace Botomag.DAL.Model.Configurations
{
    public class UserConfiguration : BaseConfiguration<User, Guid>
    {
        public UserConfiguration()
        {
            Property(n => n.PasswordHash).IsRequired();
            Property(n => n.Email).IsRequired();
        }
    }
}
