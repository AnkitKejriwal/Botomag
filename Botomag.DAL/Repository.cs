﻿using System.Data.Entity;
using Botomag.DAL.Model;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;

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
            _dbSet = context.Set<TEntity>();
        }

        public TEntity Add(TEntity entity)
        {
            TEntity result;
            result = _dbSet.Add(entity);
            return result;
        }

        public TEntity Remove(TEntity entity)
        {
            TEntity result;
            result = _dbSet.Remove(entity);
            return result;
        }

        public IQueryable<TEntity> Get()
        {
            IQueryable<TEntity> query;

            query = _dbSet.AsQueryable();

            return query;
        }

        public TEntity Find(TKey key)
        {
            TEntity entity = _dbSet.Find(key);
            return entity;
        }

        public async Task<TEntity> FindAsync(TKey key)
        {
            TEntity entity = await _dbSet.FindAsync(key);
            return entity;
        }
    }
}
