using System;
using System.Collections.Generic;

namespace Botomag.DAL.Model
{
    /// <summary>
    /// Represent bot
    /// </summary>
    public class Bot : BaseEntity<Guid>
    {
        public string Token { get; set; }

        public virtual ICollection<Command> Commands { get; set; }

        public virtual ICollection<LastUpdate> LastUpdates { get; set; }

        public virtual BotStat BotStat { get; set; }

        public virtual Response InvalidCommandResponse { get; set; }
    }
}
