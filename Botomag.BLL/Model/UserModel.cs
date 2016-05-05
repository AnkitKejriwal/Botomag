using System;

namespace Botomag.BLL.Model
{
    public class UserModel : BaseModel<Guid>
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordHash { get; set; }
    }
}
