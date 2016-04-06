using System;
using System.ComponentModel.DataAnnotations;

namespace TelegramBot.Core.Types.RequestTypes
{
    /// <summary>
    /// Represents ForceReply
    /// see: https://core.telegram.org/bots/api#forcereply
    /// </summary>
    public class ForceReplyType : BaseReplyRequest
    {
        [Required]
        public bool? Force_Reply { get; set; }

    }
}
