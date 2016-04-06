using System;

namespace TelegramBot.Core.Types.ResponseTypes
{
    /// <summary>
    /// Represents Chat type
    /// see https://core.telegram.org/bots/api#chat
    /// </summary>
    public class ChatResponse : BaseResponse
    {
        public int? Id { get; set; }

        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public string Type { get; set; }
    }
}