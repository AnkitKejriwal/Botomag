using System;
using System.Reflection;
using System.Dynamic;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TelegramBot.Core.Types.RequestTypes
{
    /// <summary>
    /// This class form dynamic object which is used for request through telegram bot API
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Request : DynamicObject
    {
        #region Properties

        private Dictionary<string, object> _params = new Dictionary<string,object>();

        #endregion Properties

        #region Constructors

        public Request(BaseRequest args)
        {
            ProcessArgs(args);
        }

        #endregion Constructors

        #region Methods

        private Dictionary<string, object> ProcessInnerArgs(object args, Type[] assemblyTypes)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args.");
            }

            if (assemblyTypes == null)
            {
                throw new ArgumentNullException("assemblyTypes.");
            }

            Dictionary<string, object> result = new Dictionary<string, object>();
            Type runtimeType = args.GetType();
            PropertyInfo[] properties = runtimeType.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(args);
                if (value == null)
                {
                    RequiredAttribute attr = property.GetCustomAttribute<RequiredAttribute>();
                    if (attr != null)
                    {
                        throw new InvalidOperationException(string.Format("Missing required parameter {0}", property.Name));
                    }
                }
                else
                {
                    RequiredAttribute attr = property.GetCustomAttribute<RequiredAttribute>();
                    if (attr != null)
                    {
                        if (value is string && string.IsNullOrEmpty((string)value))
                        {
                            throw new InvalidOperationException(string.Format("Missing required parameter {0}", property.Name));
                        }
                    }

                    Type currentPropertyType = assemblyTypes.Where(n => n == property.PropertyType).FirstOrDefault();
                    if (currentPropertyType != null)
                    {
                        Dictionary<string, object> inner_params = ProcessInnerArgs(value, assemblyTypes);
                        result.Add(property.Name.ToLower(), inner_params);
                    }
                    else
                    {
                        result.Add(property.Name.ToLower(), value);
                    }
                }
            }
            return result;
        }

        private void ProcessArgs(BaseRequest args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args.");
            }

            Type runtimeType = args.GetType();
            string typeName = runtimeType.Name;
            switch (typeName)
            {
                case "SendMessageRequest":
                    _params.Add("method", Enum.GetName(typeof(Methods), Methods.sendMessage));
                    break;
                case "GetMeRequest":
                    _params.Add("method", Enum.GetName(typeof(Methods), Methods.getMe));
                    break;
            }
            PropertyInfo[] properties = runtimeType.GetProperties();
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            Type[] types = executingAssembly.GetTypes().Where(n => n.Name.EndsWith("request", true, System.Globalization.CultureInfo.InvariantCulture)).ToArray();
            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(args);
                if (value == null)
                {
                    RequiredAttribute attr = property.GetCustomAttribute<RequiredAttribute>();
                    if (attr != null)
                    {
                        throw new InvalidOperationException(string.Format("Missing required parameter {0}", property.Name));
                    }
                }
                else
                {
                    RequiredAttribute attr = property.GetCustomAttribute<RequiredAttribute>();
                    if (attr != null)
                    {
                        if (value is string && string.IsNullOrEmpty((string)value))
                        {
                            throw new InvalidOperationException(string.Format("Missing required parameter {0}", property.Name));
                        }
                    }
                    Type currentPropertyType = types.Where(n => n == property.PropertyType).FirstOrDefault();
                    if (currentPropertyType != null)
                    {
                        Dictionary<string, object> inner_params = ProcessInnerArgs(value, types);
                        _params.Add(property.Name.ToLower(), inner_params);
                    }
                    else
                    {
                        _params.Add(property.Name.ToLower(), value);
                    }
                }
            }
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _params.Keys;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return _params.TryGetValue(binder.Name, out result);
        }

        #endregion Methods
    }
}
