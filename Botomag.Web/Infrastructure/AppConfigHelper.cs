using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Threading.Tasks;

namespace Botomag.Web.Infrastructure
{
    /// <summary>
    /// Enum for reading data from config file
    /// </summary>
    public enum AppConfigKeys
    {
        TelegramPostToken,
        BotToken,
        PartnerLink
    }
    /// <summary>
    /// Helper class for reading data from configuration file
    /// </summary>
    public class AppConfigHelper
    {
        /// <summary>
        /// Get value from app settings section in web.config file
        /// throws ArgumentNullException
        /// throws InvalidOperationException if there is no value with such key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static T GetValue<T>(AppConfigKeys key, Func<string, T> converter)
        {
            if (converter == null)
            {
                throw new ArgumentNullException("converter");
            }
            string keyName = Enum.GetName(typeof(AppConfigKeys), key);
            string strResult = ConfigurationManager.AppSettings[keyName];
            if (strResult == null)
            {
                throw new InvalidOperationException(string.Format("No value with {0} key.", keyName));
            }
            return converter(strResult);
        }

        /// <summary>
        /// Get value from app settings section in web.config file asynchronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static Task<T> GetValueAsync<T>(AppConfigKeys key, Func<string, T> converter)
        {
            if (converter == null)
            {
                throw new ArgumentNullException("converter");
            }
            return Task<T>.Factory.StartNew(() =>
                {
                    return GetValue<T>(key, converter);
                });
        }
    }
}