using System;

namespace TelegramBot.Core.Types.ResponseTypes
{
    /// <summary>
    /// Wrapper class for return types
    /// </summary>
    /// <typeparam name="TOutput"></typeparam>
    public class Response<TOutput> where TOutput : BaseResponse
    {
        public bool Ok { get; set; }
      
        public TOutput Result { get; set; }
    }
}
