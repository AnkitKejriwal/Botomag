using System;

namespace Botomag.DAL.Model
{
    /// <summary>
    /// Base class for all entities in app
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class BaseEntity<TKey> : IBaseEntity<TKey>
        where TKey : struct
    {
        public TKey Id { get; set; }
    }
}
