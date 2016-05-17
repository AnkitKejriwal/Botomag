using System;
using System.Linq;
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

        public string Name { get; set; }

        public DateTime? LastUpdate { get; set; }

        public Guid UserId { get; set; }

        public virtual User User { get; set; }
    }
}
