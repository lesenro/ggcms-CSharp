using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

    }
}