using System;

using Botomag.BLL.Model;

namespace Botomag.BLL.Contracts
{
    public interface IUserService 
    {
        bool TryCreateUser(UserModel model, out string Message);
    }
}
