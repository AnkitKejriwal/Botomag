using System;

namespace Botomag.DAL.Model
{
    public class User : BaseEntity<Guid>
    {
        public string Email { get; set; }

        public string PasswordHash { get; set; }
    }
}
