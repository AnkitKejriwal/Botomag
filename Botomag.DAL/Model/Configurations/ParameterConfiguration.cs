using System;
using System.Data.Entity.ModelConfiguration;

namespace Botomag.DAL.Model.Configurations
{
    public class ParameterConfiguration : BaseConfiguration<Parameter, Guid>
    {
        public ParameterConfiguration()
        {
            Property(n => n.Expression).IsRequired();
            HasRequired(n => n.Command).WithMany(n => n.Parameters).HasForeignKey(n => n.CommandId);
            HasRequired(n => n.Response).WithOptional();
        }
    }
}
