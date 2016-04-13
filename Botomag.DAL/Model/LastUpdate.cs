using System;

namespace Botomag.DAL.Model
{
    /// <summary>
    /// Represent last update for bot
    /// </summary>
    public class LastUpdate : BaseEntity<Guid>
    {
        public int ChatId { get; set; }

        public int UpdateId { get; set; }

        public Guid BotId { get; set; }

        public virtual Bot Bot { get; set; }

        public int CurrentState { get; set; }
    }
}
