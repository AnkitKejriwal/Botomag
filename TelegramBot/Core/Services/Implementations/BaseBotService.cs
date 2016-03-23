using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

using TelegramBot.Core.Services.Contracts;
using TelegramBot.Core.Types.MethodParamTypes;
using TelegramBot.Core.Types.ReturnTypes;

namespace TelegramBot.Core.Services.Implementations
{
    /// <summary>
    /// Base implementation of Telegram Bot API Service
    /// </summary>
    public class BaseBotService : IBaseBotService
    {
        #region Constructors

        /// <summary>
        /// Token of telegram bot
        /// </summary>
        /// <param name="token"></param>
        public BaseBotService(string token)
        {
            Token = token;
        }

        #endregion Constructors

        #region Private Properties And Fields

        private const string _url = "https://api.telegram.org/bot";

        #endregion Private Properties And Fields

        #region Public Properties And Fields

        public string Token { get; private set; }

        #endregion

        /// <summary>
        /// Form url for request
        /// </summary>
        /// <param name="method"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private string ProcessGetArgs(string method, Dictionary<string, string> param = null)
        {
            if (string.IsNullOrEmpty(method))
            {
                throw new ArgumentNullException("method empty or null.");
            }

            // Form request string
            string url = _url + Token + "/" + method;

            if (param != null && param.Count > 0)
            {
                string paramSeparator = "?";
                StringBuilder strb = new StringBuilder();

                foreach (KeyValuePair<string, string> pair in param)
                {
                    strb.Append(paramSeparator + pair.Key + "=" + pair.Value);
                    paramSeparator = "&";
                }

                url += strb.ToString();
            }

            return url;
        }

        /// <summary>
        /// Check required method parameters and wrap it to dictionary
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private Dictionary<string, string> CheckParamsAndFormDict(BaseMethodParamType args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            // Check requred params
            PropertyInfo[] properties = args.GetType().GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                RequiredAttribute req = prop.GetCustomAttribute<RequiredAttribute>();
                if (req != null)
                {
                    object val = prop.GetValue(args);
                    if (val == null)
                    {
                        throw new InvalidOperationException(string.Format("{0} is required", prop.Name));
                    }
                    if (val is string && string.IsNullOrEmpty((string)val))
                    {
                        throw new InvalidOperationException(string.Format("{0} is required", prop.Name));
                    }
                }
            }

            // Form dictionary of parameters
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (PropertyInfo prop in properties)
            {
                object propValue = prop.GetValue(args);
                if (propValue != null)
                {
                    dict.Add(prop.Name, propValue.ToString());
                }
            }
            return dict;
        }

        /// <summary>
        /// Base method for GET HTTP method to telegram bot API
        /// throws ArgumentNullException if method is null or empty
        /// </summary>
        /// <param name="method"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public string Get(string method, Dictionary<string, string> param = null)
        {
            if (string.IsNullOrEmpty(method))
            {
                throw new ArgumentNullException("method empty or null.");
            }

            // Form request string
            string url = ProcessGetArgs(method, param);

            // Make request
            WebRequest request = WebRequest.Create(url);

            string output;

            using (WebResponse response = request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                output = reader.ReadToEnd();
            }

            return output;
        }

        /// <summary>
        /// Base async method for GET HTTP method to telegram bot API
        /// </summary>
        /// <param name="method"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<string> GetAsync(string method, Dictionary<string, string> param = null)
        {
            if (string.IsNullOrEmpty(method))
            {
                throw new ArgumentNullException("method empty or null.");
            }

            // Form request string
            string url = ProcessGetArgs(method, param);

            // Make request
            WebRequest request = WebRequest.Create(url);

            string output;

            using (WebResponse response = await request.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                output = await reader.ReadToEndAsync();
            }

            return output;
        }

        /// <summary>
        /// Implementation getMe GET method
        /// see https://core.telegram.org/bots/api#getme
        /// </summary>
        /// <returns></returns>
        public Response<User> getMe()
        {
            string methodName = Enum.GetName(typeof(Methods), Methods.getMe);

            string result = Get(methodName);

            Response<User> response = JsonConvert.DeserializeObject<Response<User>>(result);

            return response;
        }

        /// <summary>
        /// Implementation getMe GET method asynchronously
        /// </summary>
        /// <returns></returns>
        public async Task<Response<User>> getMeAsync()
        {
            string methodName = Enum.GetName(typeof(Methods), Methods.getMe);

            string result = await GetAsync(methodName);

            Response<User> response = await Task<Response<User>>.Factory.StartNew(() =>
                {
                    return JsonConvert.DeserializeObject<Response<User>>(result);
                });
            return response;
        }

        /// <summary>
        /// Read message from telegram API and deserialize result
        /// use when set webhook
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public TOutput ReadMessage<TOutput>(Stream stream)
        {
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            TOutput result;
            using (StreamReader reader = new StreamReader(stream))
            {
                string rawResult = reader.ReadToEnd();
                result = JsonConvert.DeserializeObject<TOutput>(rawResult);
            }
            return result;
        }

        /// <summary>
        /// Read message from telegram API and deserialize result
        /// use when set webhook
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task<TOutput> ReadMessageAsync<TOutput>(Stream stream)
        {
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            string rawResult;
            using (StreamReader reader = new StreamReader(stream))
            {
                rawResult = await reader.ReadToEndAsync();
            }
            return await Task<TOutput>.Factory.StartNew(() =>
                {
                    return JsonConvert.DeserializeObject<TOutput>(rawResult);
                });
        }
    }
}
