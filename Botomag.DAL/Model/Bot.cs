using System;
using System.Collections.Generic;

namespace Botomag.DAL.Model
{
    /// <summary>
    /// Represent bot through its token
    /// </summary>
    public class Bot : BaseEntity<Guid>
    {
        public string Token { get; set; }

        public virtual ICollection<Command> Commands { get; set; }

        public virtual ICollection<LastUpdate> LastUpates { get; set; }
    }
}
