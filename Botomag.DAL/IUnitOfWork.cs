using System;
using System.Collections.Generic;
using Botomag.DAL.Model;

namespace Botomag.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        Repository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : BaseEntity<TKey>
            where TKey : struct;

        int Save();
    }
}
