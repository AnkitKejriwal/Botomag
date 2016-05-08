using System;
using System.Collections.Generic;
namespace Botomag.DAL.Model
{
    public class User : BaseEntity<Guid>
    {
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public virtual ICollection<Bot> Bots { get; set; }
    }
}
