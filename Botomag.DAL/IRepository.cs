using Botomag.DAL.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Botomag.DAL
{
    public interface IRepository<TEntity, TKey> where TEntity : IBaseEntity<TKey> where TKey : struct 
    {
        TEntity Add(TEntity entity);

        TEntity Remove(TEntity entity);

        void Update(TEntity entity);

        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null,
                    params Expression<Func<TEntity, object>>[] propertiesInclude);
    }
}
