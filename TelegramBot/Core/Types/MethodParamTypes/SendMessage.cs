using System.ComponentModel.DataAnnotations;

namespace TelegramBot.Core.Types.MethodParamTypes
{
    /// <summary>
    /// Represents type for getMe method parameters
    /// see: https://core.telegram.org/bots/api#getme
    /// </summary>
    public class SendMessage : BaseMethodParamType
    {
        [Required]
        public string chat_id { get; set; }

        [Required]
        public string text { get; set; }

        public string parse_mode { get; set; }

        public bool? disable_web_page_preview { get; set; }

        public bool? disable_notification { get; set; }

        public int? reply_to_message_id { get; set; }

        public BaseReplyType reply_markup { get; set; }
    }
}
