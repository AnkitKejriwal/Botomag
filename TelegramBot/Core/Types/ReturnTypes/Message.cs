using System.Collections.Generic;

namespace TelegramBot.Core.Types.ReturnTypes
{

    /// <summary>
    /// Represents Message type
    /// see: https://core.telegram.org/bots/api#message
    /// </summary>
    public class Message
    {
        public int Message_Id { get; set; }

        public User From { get; set; }

        public Chat Chat { get; set; }

        public int Date { get; set; }

        public string Text { get; set; }

    }
}
