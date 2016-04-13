using System.ComponentModel.DataAnnotations;

namespace TelegramBot.Core.Types.RequestTypes
{
    /// <summary>
    /// Represents type for sendMessage method parameters
    /// see: https://core.telegram.org/bots/api#sendmessage
    /// </summary>
    public class SendMessageRequest : BaseRequest
    {
        [Required]
        public int? Chat_Id { get; set; }

        [Required]
        public string Text { get; set; }

        public string Parse_Mode { get; set; }

        public bool? Disable_Web_Page_Preview { get; set; }

        public bool? Disable_Notification { get; set; }

        public int? Reply_To_Message_Id { get; set; }

        public BaseReplyRequest Reply_Markup { get; set; }
    }
}
