using Botomag.DAL.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Botomag.DAL
{
    public interface IRepository<TEntity, TKey> where TEntity : IBaseEntity<TKey> where TKey : struct 
    {
        TEntity Add(TEntity entity);

        TEntity Remove(TEntity entity);

        IQueryable<TEntity> Get();

        TEntity Find(TKey key);

        Task<TEntity> FindAsync(TKey key);
    }
}
