namespace TelegramBot.Core.Types.ReturnTypes
{
    /// <summary>
    /// Represents User type
    /// see: https://core.telegram.org/bots/api#user
    /// </summary>
    public class User : BaseReturnType
    {
        public int Id { get; set; }

        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public string UserName { get; set; }
    }
}
