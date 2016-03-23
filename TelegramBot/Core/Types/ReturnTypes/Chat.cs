namespace TelegramBot.Core.Types.ReturnTypes
{
    /// <summary>
    /// Represents Chat type
    /// see https://core.telegram.org/bots/api#chat
    /// </summary>
    public class Chat : BaseReturnType
    {
        public int Id { get; set; }

        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public string Type { get; set; }
    }
}
