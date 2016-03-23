using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

using TelegramBot.Core.Types.ReturnTypes;
using TelegramBot.Core.Types.MethodParamTypes;

namespace TelegramBot.Core.Services.Contracts
{
    public interface IBaseBotService
    {
        string Token { get; }

        string Get(string method, Dictionary<string, string> param = null);

        Task<string> GetAsync(string method, Dictionary<string, string> param = null);

        Response<User> getMe();

        Task<Response<User>> getMeAsync();

        TOutput ReadMessage<TOutput>(Stream stream);

        Task<TOutput> ReadMessageAsync<TOutput>(Stream stream);
    }
}
