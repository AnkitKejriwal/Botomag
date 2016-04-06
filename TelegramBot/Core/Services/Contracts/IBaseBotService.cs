using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

using TelegramBot.Core.Types.RequestTypes;
using TelegramBot.Core.Types.ResponseTypes;

namespace TelegramBot.Core.Services.Contracts
{
    public interface IBaseBotService
    {
        TOutput ReadMessage<TOutput>(Stream stream, string token) where TOutput : BaseResponse;

        Task<TOutput> ReadMessageAsync<TOutput>(Stream stream, string token) where TOutput : BaseResponse;

        Response<TOutput> Post<TOutput>(string botToken, Request request) where TOutput : BaseResponse;

        Task<Response<TOutput>> PostAsync<TOutput>(string botToken, Request request) where TOutput : BaseResponse;
    }
}
