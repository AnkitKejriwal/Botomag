using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Configuration;

namespace Botomag.Web.Infrastructure
{
    /// <summary>
    /// Defines keys for application state cache
    /// </summary>
    public enum CacheKeys
    {
        TelegramPostToken,
        PartnerLink,
        BotName
    }
    /// <summary>
    /// Class for working with application cache
    /// </summary>
    ///
    public class CacheHelper
    {
        /// <summary>
        /// Set value in application cache
        /// throws ArgumentNullException
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="applicationState"></param>
        /// <returns>The same value as val param</returns>
        public static T SetValue<T>(CacheKeys key, T val, HttpApplicationStateBase applicationState)
        {
            if (applicationState == null)
            {
                throw new ArgumentNullException("applicationState");
            }
            string keyName = Enum.GetName(typeof(CacheKeys), key);
            applicationState.Lock();
            applicationState[keyName] = val;
            applicationState.UnLock();
            return val;
        }

        /// <summary>
        /// Set value in application cache asynchronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="applicationState"></param>
        /// <returns></returns>
        public static Task<T> SetValueAsync<T>(CacheKeys key, T val, HttpApplicationStateBase applicationState)
        {
            if (applicationState == null)
            {
                throw new ArgumentNullException("applicationState");
            }
            return Task<T>.Factory.StartNew(() =>
                {
                    return SetValue<T>(key, val, applicationState);
                });
        }

        /// <summary>
        /// Get value from application cache
        /// throws ArgumentNullException
        /// throws KeyNotFoundException if key not exist in cache values collection
        /// throws InvalidCastException if value can`t be cast to T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="applicationState"></param>
        /// <returns></returns>
        public static T GetValue<T>(CacheKeys key, HttpApplicationStateBase applicationState)
        {
            if (applicationState == null)
            {
                throw new ArgumentNullException("applicationState");
            }
            string keyName = Enum.GetName(typeof(CacheKeys), key);
            if (!applicationState.AllKeys.Contains(keyName))
            {
                throw new KeyNotFoundException(keyName);
            }
            T result = (T)applicationState[keyName];
            return result;
        }

        /// <summary>
        /// Get value from application cache asynchronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="applicationState"></param>
        /// <returns></returns>
        public static Task<T> GetValueAsync<T>(CacheKeys key, HttpApplicationStateBase applicationState)
        {
            if (applicationState == null)
            {
                throw new ArgumentNullException("applicationState");
            }
            return Task<T>.Factory.StartNew(() =>
                {
                    return GetValue<T>(key, applicationState);
                });
        } 

        /// <summary>
        /// Get value from cache, if not exist, get it through valuator, set value and return result
        /// </summary>
        /// <returns></returns>
        public static T GetOrSet<T>(CacheKeys key, HttpApplicationStateBase applicationState, Func<T> valuator)
        {
            if (applicationState == null)
            {
                throw new ArgumentNullException("applicationState");
            }
            if (valuator == null)
            {
                throw new ArgumentNullException("valuator");
            }
            T result;
            try
            {
                result = GetValue<T>(key, applicationState);
            }
            catch (KeyNotFoundException)
            {
                result = valuator();
                SetValue<T>(key, result, applicationState);
            }
            return result;
        }

        /// <summary>
        /// Get value from cache, if not exist, get it through valuator, set value and return result asynchronously
        /// </summary>
        /// <returns></returns>
        public static Task<T> GetOrSetAsync<T>(CacheKeys key, HttpApplicationStateBase applicationState, Func<T> valuator)
        {
            if (applicationState == null)
            {
                throw new ArgumentNullException("applicationState");
            }
            if (valuator == null)
            {
                throw new ArgumentNullException("valuator");
            }

            Task<T> task = GetValueAsync<T>(key, applicationState);
            task.ContinueWith(t =>
                {
                    return t.Result;
                }, TaskContinuationOptions.OnlyOnRanToCompletion);
            task.ContinueWith(t =>
                {
                    Task<T> valueTask = Task<T>.Factory.StartNew(() => valuator());
                    valueTask.ContinueWith(vt =>
                        {
                            SetValueAsync<T>(key, vt.Result, applicationState);
                            return vt.Result;
                        });
                }, TaskContinuationOptions.OnlyOnFaulted);
            return task;
        }
    }
}