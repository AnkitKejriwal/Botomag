using System;

namespace TelegramBot.Core.Types.ResponseTypes
{
    /// <summary>
    /// Represents User type
    /// see: https://core.telegram.org/bots/api#user
    /// </summary>
    public class UserResponse : BaseResponse
    {
        public int? Id { get; set; }

        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public string UserName { get; set; }
    }
}