namespace TelegramBot.Core.Types.MethodParamTypes
{
    /// <summary>
    /// Represents ReplyKeyboardMarkup type
    /// see: https://core.telegram.org/bots/api#replykeyboardmarkup
    /// </summary>
    public class ReplyKeyboardMarkup : BaseReplyType
    {
        public string[][] keyboard { get; set; }

        public bool? resize_keyboard { get; set; }

        public bool? one_time_keyboard { get; set; }

        public bool? selective { get; set; }
    }
}
