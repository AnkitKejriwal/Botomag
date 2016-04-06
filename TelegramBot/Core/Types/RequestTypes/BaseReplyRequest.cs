using System;

namespace TelegramBot.Core.Types.RequestTypes
{
    /// <summary>
    /// Base method for reply types
    /// </summary>
    public abstract class BaseReplyRequest
    {
        public bool? Selective { get; set; }
    }
}
