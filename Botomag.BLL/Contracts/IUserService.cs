using System;

using Botomag.BLL.Models;

namespace Botomag.BLL.Contracts
{
    public interface IUserService 
    {
        CreateUserResult CreateUser(UserModel model);
    }
}
