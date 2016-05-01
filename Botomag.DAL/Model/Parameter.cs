using System;

namespace Botomag.DAL.Model
{
    /// <summary>
    /// Present Parameter(s) of appropriate command
    /// it can be a set of parameter (set of tokens divide by spaces)
    /// or it can be reqular expression
    /// </summary>
    public class Parameter : BaseEntity<Guid>
    {
        public string Expression { get; set; }

        public ParameterTypes Type { get; set; }

        public virtual Response Response { get; set; }

        public Guid CommandId { get; set; }

        public virtual Command Command { get; set; }
    }
}
