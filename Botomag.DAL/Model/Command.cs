using System;

namespace Botomag.DAL.Model
{
    public class Command : BaseEntity<Guid>
    {
        public Guid BotId { get; set; }

        public virtual Bot Bot { get; set; }

        public string Name { get; set; }

        public CommandTypes CommandType { get; set; }

        public int CurrentState { get; set; }

        public int NextState { get; set; }

        public Guid ResponseId { get; set; }

        public virtual Response Response { get; set; }


    }
}
