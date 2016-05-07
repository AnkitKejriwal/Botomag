using System;
using System.Threading.Tasks;

using Botomag.BLL.Models;

namespace Botomag.BLL.Contracts
{
    public interface IUserService 
    {
        Task<CreateUserResult> CreateUserAsync(UserModel model);
    }
}
