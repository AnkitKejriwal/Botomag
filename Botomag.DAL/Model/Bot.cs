using System;

namespace Botomag.DAL.Model
{
    /// <summary>
    /// Class for store bot features before sessions
    /// and app restarts
    /// </summary>
    public class Bot : BaseEntity<Guid>
    {
        public string Token { get; set; }

        public int LastUpdate { get; set; }
    }
}
