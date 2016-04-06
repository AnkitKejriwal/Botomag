using System;
using System.ComponentModel.DataAnnotations;

namespace TelegramBot.Core.Types.RequestTypes
{
    /// <summary>
    /// Represents ReplyKeyboardHide type
    /// see: https://core.telegram.org/bots/api#replykeyboardhide
    /// </summary>
    public class ReplyKeyboardHideRequest : BaseReplyRequest
    {
        [Required]
        public bool? Hide_Keyboard { get; set; }
    }
}
