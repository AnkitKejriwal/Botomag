using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Botomag.DAL.Model;

namespace Botomag.DAL
{
    /// <summary>
    /// This class implements unit of work pattern
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {

        #region Private Members

        private Context _context;

        private Dictionary<Type, object> _repos;

        #endregion Private Members

        #region Constructors

        public UnitOfWork()
        {
            _context = new Context();
            _repos = new Dictionary<Type, object>();
        }

        #endregion Constructors

        public Repository<TEntity, TKey> GetRepository<TEntity, TKey>() 
            where TEntity : BaseEntity<TKey> 
            where TKey : struct
        {
            if (!_repos.ContainsKey(typeof(TEntity)))
            {
                _repos.Add(typeof(TEntity), new Repository<TEntity, TKey>(_context));
            }

            return _repos[typeof(TEntity)] as Repository<TEntity, TKey>;
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
                _context = null;
                GC.SuppressFinalize(this);
            }
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
