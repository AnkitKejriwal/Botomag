using System;

using Botomag.BLL.Models;

namespace Botomag.BLL.Contracts
{
    public class VerifyUserResult
    {
        public UserModel User { get; set; }

        public bool IsValid { get; set; }
    }
}
