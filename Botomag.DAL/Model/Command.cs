using System;
using System.Collections.Generic;

namespace Botomag.DAL.Model
{
    /// <summary>
    /// Represent command of bot
    /// </summary>
    public class Command : BaseEntity<Guid>
    {
        public Guid BotId { get; set; }

        public virtual Bot Bot { get; set; }

        public string Name { get; set; }

        public int CurrentState { get; set; }

        public int NextState { get; set; }

        public virtual ICollection<Parameter> Parameters { get; set; }

        public virtual Response InvalidParameterResponse { get; set; }
    }
}
