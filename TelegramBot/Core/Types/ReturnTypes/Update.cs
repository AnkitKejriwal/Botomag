namespace TelegramBot.Core.Types.ReturnTypes
{
    /// <summary>
    /// Represents Update type
    /// see: https://core.telegram.org/bots/api#update
    /// </summary>
    public class Update : BaseReturnType
    {
        public int Update_Id { get; set; }

        public Message Message { get; set; }
    }
}
