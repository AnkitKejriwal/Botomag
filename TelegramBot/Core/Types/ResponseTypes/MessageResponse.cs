using System;

namespace TelegramBot.Core.Types.ResponseTypes
{

    /// <summary>
    /// Represents Message type
    /// see: https://core.telegram.org/bots/api#message
    /// </summary>
    public class MessageResponse : BaseResponse
    {
        public int? Message_Id { get; set; }

        public UserResponse From { get; set; }

        public ChatResponse Chat { get; set; }

        public int? Date { get; set; }

        public string Text { get; set; }

    }
}