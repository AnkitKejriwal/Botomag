namespace TelegramBot.Core.Types.MethodParamTypes
{
    /// <summary>
    /// Represents ForceReply
    /// see: https://core.telegram.org/bots/api#forcereply
    /// </summary>
    public class ForceReply : BaseReplyType
    {
        public bool force_reply { get; set; }

        public bool? selective { get; set; }
    }
}
