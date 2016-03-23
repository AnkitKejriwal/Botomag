using System.Data.Entity;
using Botomag.DAL.Model;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System;

namespace Botomag.DAL
{
    /// <summary>
    /// Implements repository pattern
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public sealed class Repository<TEntity, TKey> : IRepository<TEntity, TKey> 
        where TEntity : BaseEntity<TKey>
        where TKey : struct
    {

        #region Private Fields

        private Context _context;

        private DbSet<TEntity> _dbSet;

        #endregion Private Fields

        public Repository(Context context)
        {
            _context = context;
            lock(_context)
            {
                _dbSet = context.Set<TEntity>();
            }
        }

        public TEntity Add(TEntity entity)
        {
            TEntity result;
            lock(_context)
            {
                result = _dbSet.Add(entity);
            }
            return result;
        }

        public TEntity Remove(TEntity entity)
        {
            TEntity result;
            lock(_context)
            {
                result = _dbSet.Remove(entity);
            }
            return result;
        }

        public void Update(TEntity entity)
        {
            lock(_context)
            {
                DbEntityEntry entry = _context.Entry<TEntity>(entity);
                if (entry.State == EntityState.Detached)
                {
                    _dbSet.Attach(entity);
                }
                entry.State = EntityState.Modified;
            }
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null, 
            params Expression<Func<TEntity, object>>[] propertiesInclude)
        {
            IQueryable<TEntity> query;
            lock(_context)
            {
                query = _dbSet.AsQueryable();
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            foreach (Expression<Func<TEntity, object>> property in propertiesInclude)
            {
                query = query.Include(property);
            }

            return query;
        }
    }
}
