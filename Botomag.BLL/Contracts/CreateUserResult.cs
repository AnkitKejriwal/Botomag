using System;

using Botomag.BLL.Models;

namespace Botomag.BLL.Contracts
{
    public class CreateUserResult
    {
        public CreateUserResult(UserModel user, string message, bool isSuccess)
        {
            User = user;
            Message = message;
            IsSuccess = isSuccess;
        }

        public UserModel User { get; private set; }

        public string Message { get; private set; }

        public bool IsSuccess { get; private set; }
    }
}
