using System;
using System.ComponentModel.DataAnnotations;

namespace TelegramBot.Core.Types.RequestTypes
{
    /// <summary>
    /// Represents ReplyKeyboardMarkup type
    /// see: https://core.telegram.org/bots/api#replykeyboardmarkup
    /// </summary>
    public class ReplyKeyboardMarkupRequest : BaseReplyRequest
    {
        [Required]
        public string[][] Keyboard { get; set; }

        public bool? Resize_Keyboard { get; set; }

        public bool? One_Time_Keyboard { get; set; }
    }
}
