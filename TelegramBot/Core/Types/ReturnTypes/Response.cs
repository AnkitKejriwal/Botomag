namespace TelegramBot.Core.Types.ReturnTypes
{
    /// <summary>
    /// Wrapper class for return types
    /// </summary>
    /// <typeparam name="TOutput"></typeparam>
    public class Response<TOutput> where TOutput : BaseReturnType
    {
        public bool Ok { get; set; }
      
        public TOutput Result { get; set; }
    }
}
