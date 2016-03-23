namespace TelegramBot.Core.Types.MethodParamTypes
{
    /// <summary>
    /// Represents ReplyKeyboardHide type
    /// see: https://core.telegram.org/bots/api#replykeyboardhide
    /// </summary>
    public class ReplyKeyboardHide : BaseReplyType
    {
        public bool hide_keyboard { get; set; }

        public bool? selective { get; set; }
    }
}
