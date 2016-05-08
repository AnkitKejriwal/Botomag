using System;
using System.Collections.Generic;

namespace Botomag.BLL.Models
{
    public class UserModel : BaseModel<Guid>
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordHash { get; set; }
    }
}
