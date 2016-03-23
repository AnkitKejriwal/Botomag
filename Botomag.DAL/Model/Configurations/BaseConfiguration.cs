using System;
using System.Data.Entity.ModelConfiguration;

namespace Botomag.DAL.Model
{
    public class BaseConfiguration<TEntity, TKey> : EntityTypeConfiguration<TEntity> 
        where TEntity : BaseEntity<TKey> 
        where TKey : struct
    {
        public BaseConfiguration()
        {
            HasKey(n => n.Id);
        }
    }
}
