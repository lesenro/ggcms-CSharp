using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;

namespace GgcmsCSharp.Utils
{
    public class Tools
    {
        public static bool IsPropertyExist(dynamic settings, string name)
        {
            if (settings is ExpandoObject)
                return ((IDictionary<string, object>)settings).ContainsKey(name);

            return settings.GetType().GetProperty(name) != null;
        }
        public static string getMd5Hash(string input)
        {
            //Check wether data was passed
            if ((input == null) || (input.Length == 0))
            {
                return String.Empty;
            }

            //Calculate MD5 hash. This requires that the string is splitted into a byte[].
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] textToHash = Encoding.Default.GetBytes(input);
            byte[] result = md5.ComputeHash(textToHash);
            //Convert result back to string.
            return System.BitConverter.ToString(result).Replace("-", "").ToLower();
        }
        public static bool IsInt(string val)
        {
            return Regex.IsMatch(val, "^(\\-|\\+)?\\d+$", RegexOptions.Singleline);
        }
        public static int parseInt(string s, int defaultVal = 0)
        {
            int i = defaultVal;
            if (int.TryParse(s, out i))
            {
                return i;
            }
            else
            {
                return defaultVal;
            }
        }

        public static string getGuid()
        {
            return Guid.NewGuid().ToString();
        }
        public static string getRandNumber(int len)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            StringBuilder sb = new StringBuilder(len);
            for (int i = 0; i < len; i++)
            {
                sb.Append(rand.Next(10));
            }
            return sb.ToString();
        }
        public static string getRandString(int len=0)
        {
            string s = getMd5Hash(getGuid());
            if (len == 0|| len > s.Length)
            {
                return s;
            }
            return s.Substring(0, len);
        }
        public static T ConvertType<T>(object val, T defaultValue)
        {
            if (val == null) return defaultValue;//返回类型的默认值
            Type tp = typeof(T);
            //泛型Nullable判断，取其中的类型
            if (tp.IsGenericType)
            {
                tp = tp.GetGenericArguments()[0];
            }
            //string直接返回转换
            if (tp.Name.ToLower() == "string")
            {
                return (T)val;
            }
            //反射获取TryParse方法
            var TryParse = tp.GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder,
                                            new Type[] { typeof(string), tp.MakeByRefType() },
                                            new ParameterModifier[] { new ParameterModifier(2) });
            var parameters = new object[] { val, Activator.CreateInstance(tp) };
            bool success = (bool)TryParse.Invoke(null, parameters);
            //成功返回转换后的值，否则返回类型的默认值
            if (success)
            {
                return (T)parameters[1];
            }
            return defaultValue;
        }
        /// <summary>
        /// Json字符串转对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <returns>返回对象</returns>
        public static T JsonDeserialize<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                return (T)Activator.CreateInstance(typeof(T));
            }
        }
        /// <summary>
        /// 对象转Json字符串
        /// </summary>
        /// <param name="obj">需要转换的对象</param>
        /// <returns>返回json字符串</returns>
        public static string JsonSerialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        public static T ConvertType<T>(string val) where T : class
        {
            if (val == null) return null;//返回类型的默认值
            try
            {
                return JsonDeserialize<T>(val);
            }
            catch
            {
                return null;
            }
        }

        public static dynamic ObjectMerge(params dynamic[] objs)
        {
            Dictionary<string, dynamic> newObj = new Dictionary<string, dynamic>();
            foreach (dynamic obj in objs)
            {
                Type tp = obj.GetType();
                foreach (var prop in tp.GetProperties())
                {
                    newObj[prop.Name] = prop.GetValue(obj);
                }
            }
            return newObj;
        }
        public static string JsonStringify(dynamic obj)
        {
            return JsonSerialize(obj);
        }

    }
}