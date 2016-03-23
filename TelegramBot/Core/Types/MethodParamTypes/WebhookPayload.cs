using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Core
{
    /// <summary>
    /// This type for send response through webhook
    /// </summary>
    public class WebhookPayload
    {
        public string method { get; set; }

        public int chat_id { get; set; }

        public int? reply_to_message_id { get; set; }

        public string text { get; set; }

        public string parse_mode { get; set; }
    }
}
