using System;
using System.Data.Entity.ModelConfiguration;

namespace Botomag.DAL.Model.Configurations
{
    public class ResponseConfiguration : BaseConfiguration<Response, Guid>
    {
        public ResponseConfiguration()
        {
            Property(n => n.Text).IsRequired();
        }
    }
}
