using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;

using TelegramBot.Core.Services.Contracts;
using TelegramBot.Core.Types.ResponseTypes;
using TelegramBot.Core.Types.RequestTypes;

namespace TelegramBot.Core.Services.Implementations
{
    /// <summary>
    /// Base implementation of Telegram Bot API Service
    /// </summary>
    public class TelegramBotService : ITelegramBotService
    {

        #region Properties

        private const string _url = "https://api.telegram.org/bot";

        #endregion Properties

        #region Methods

        /// <summary>
        /// Read stream of request body from telegram API and deserialize result
        /// use when webhook is set
        /// </summary>
        /// <typeparam name="TOutput">Any type derived from BaseResponse</typeparam>
        /// <param name="stream">Stream of request body</param>
        /// <returns>BaseResponse derived type with filled properties from request body stream or null if stream can`t
        /// be deserialize into object of parameter type</returns>
        /// <exception>ArgumentNullException when stream is null and token in null or empty</exception>
        public TOutput ReadMessage<TOutput>(Stream stream) where TOutput : BaseResponse
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream.");
            }

            stream.Seek(0, SeekOrigin.Begin);
            TOutput result;
            using (StreamReader reader = new StreamReader(stream))
            {
                string rawResult = reader.ReadToEnd();
                result = JsonConvert.DeserializeObject<TOutput>(rawResult);
            }
            return result;
        }

        /// <summary>
        /// Read stream of request body from telegram API and deserialize result asynchronously
        /// use when webhook is set
        /// </summary>
        /// <typeparam name="TOutput">Any type derived from BaseResponse</typeparam>
        /// <param name="stream">Stream of request body</param>
        /// <returns>BaseResponse derived type with filled properties from request body stream or null if stream can`t
        /// be deserialize into object of parameter type</returns>
        /// <exception>ArgumentNullException when stream is null and token in null or empty</exception>
        public async Task<TOutput> ReadMessageAsync<TOutput>(Stream stream) where TOutput : BaseResponse
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream.");
            }

            stream.Seek(0, SeekOrigin.Begin);
            TOutput result;
            using (StreamReader reader = new StreamReader(stream))
            {
                // string rawResult = reader.ReadToEnd();
                string rawResult = await reader.ReadToEndAsync();
                result = await Task<TOutput>.Factory.StartNew(() =>
                {
                    return JsonConvert.DeserializeObject<TOutput>(rawResult);
                });
            }
            return result;
        }

        /// <summary>
        /// Base method for POST HTTP method to telegram bot API
        /// </summary>
        /// <typeparam name="TOutput">Type derived from BaseResponse</typeparam>
        /// <param name="botToken">Unique token of bot</param>
        /// <param name="request">Request object, contains method name and request body</param>
        /// <returns>Response object with deserialize response from telegram bot API</returns>
        /// <exception>ArgumentNullException if botToke is null or empty or request is null</exception>
        public Response<TOutput> Post<TOutput>(string botToken, Request request) where TOutput : BaseResponse
        {
            if (string.IsNullOrEmpty(botToken))
            {
                throw new ArgumentNullException("botToken empty or null.");
            }
            if (request == null)
            {
                throw new ArgumentNullException("request.");
            }

            string url = _url + botToken + "/" + ((dynamic)request).method;

            // Make request
            WebRequest webRequest = WebRequest.Create(url);
            webRequest.ContentType = "application/json";
            webRequest.Method = "POST";

            string serializeRequestBody = JsonConvert.SerializeObject(request);

            using (StreamWriter writer = new StreamWriter(webRequest.GetRequestStream()))
            {
                writer.Write(serializeRequestBody);
                writer.Flush();
                writer.Close();
            }

            string output;

            Response<TOutput> result = null;
            using (WebResponse response = webRequest.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {             
                output = reader.ReadToEnd();
                result = JsonConvert.DeserializeObject<Response<TOutput>>(output);
            }

            return result;
        }

        /// <summary>
        /// Base method for POST HTTP method to telegram bot API asynchronously
        /// </summary>
        /// <typeparam name="TOutput">Type derived from BaseResponse</typeparam>
        /// <param name="botToken">Unique token of bot</param>
        /// <param name="request">Request object, contains method name and request body</param>
        /// <returns>Response object with deserialize response from telegram bot API</returns>
        /// <exception>ArgumentNullException if botToke is null or empty or request is null</exception>
        public async Task<Response<TOutput>> PostAsync<TOutput>(string botToken, Request request) where TOutput : BaseResponse
        {
            if (string.IsNullOrEmpty(botToken))
            {
                throw new ArgumentNullException("botToken empty or null.");
            }
            if (request == null)
            {
                throw new ArgumentNullException("request.");
            }

            string url = _url + botToken + "/" + ((dynamic)request).method;

            // Make request
            WebRequest webRequest = WebRequest.Create(url);
            webRequest.ContentType = "application/json";
            webRequest.Method = "POST";

            string serializeRequestBody = await Task.Factory.StartNew(() =>
            {
                return JsonConvert.SerializeObject(request);
            });

            using (Stream stream = await webRequest.GetRequestStreamAsync())
            using (StreamWriter writer = new StreamWriter(stream))
            {
                await writer.WriteAsync(serializeRequestBody);
                await writer.FlushAsync();
                writer.Close();
            }

            string output;

            Response<TOutput> result = null;
            using (WebResponse response = await webRequest.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                output = await reader.ReadToEndAsync();
                result = await Task<Response<TOutput>>.Factory.StartNew(() =>
                    {
                        return JsonConvert.DeserializeObject<Response<TOutput>>(output);
                    });
            }

            return result;
        }

        public string SerializeRequest(Request request)
        {
            return JsonConvert.SerializeObject(request);
        }

        #endregion Methods
    }
}
