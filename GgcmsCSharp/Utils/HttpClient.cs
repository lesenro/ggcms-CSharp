using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace GgcmsCSharp.Utils
{
    public enum UserAgentTypes
    {
        none, android, iphone, ipad, chrome, edeg, wp10, ie11, ie10, ie9, ie8, ie7, ie6, firefox, opera
    }
    public class HttpClient : System.Net.WebClient
    {
        public HttpClient()
           : base()
        {
            this.Encoding = Encoding.UTF8;
        }
        public void SetProxy(string pxy)
        {
            if (string.IsNullOrWhiteSpace(pxy))
            {
                return;
            }
            WebProxy proxy = new WebProxy();
            proxy = new WebProxy();
            proxy.UseDefaultCredentials = false;
            proxy.Address = new Uri(pxy);
            this.Proxy = proxy;
        }
        public UserAgentTypes UserAgent { get; set; } = UserAgentTypes.none;
        public int Timeout { get; set; } = 45000;
        public CookieContainer CookieContainer { get; set; } = new CookieContainer();
        public string ContentType { get; set; } = "application/json";

        public X509Certificate ClientCert { get; set; }


        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest r = base.GetWebRequest(address);
            var request = r as HttpWebRequest;
            if (request != null)
            {
                request.CookieContainer = this.CookieContainer;
            }
            if (Proxy != null)
            {
                request.Proxy = Proxy;
            }
            if (UserAgent != UserAgentTypes.none)
            {
                request.UserAgent = GetUserAgent(UserAgent);
            }
            if (ClientCert != null)
            {
                request.ClientCertificates.Add(ClientCert);
            }
            request.ContentType = ContentType;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.Timeout = Timeout;
            return r;
        }
        protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
        {
            WebResponse response = base.GetWebResponse(request, result);
            ReadCookies(response);
            return response;
        }
        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse response = base.GetWebResponse(request);
            ReadCookies(response);
            return response;
        }
        private void ReadCookies(WebResponse r)
        {
            var response = r as HttpWebResponse;
            if (response != null)
            {
                CookieCollection cookies = response.Cookies;
                this.CookieContainer.Add(cookies);
            }
        }
        protected override void OnDownloadStringCompleted(DownloadStringCompletedEventArgs e)
        {
            base.OnDownloadStringCompleted(e);
        }
        public string GetUserAgent(UserAgentTypes uatype)
        {

            switch (uatype)
            {
                case UserAgentTypes.android:
                    return "Mozilla/5.0 (Linux; Android 5.0; SM-G900P Build/LRX21T) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Mobile Safari/537.36";
                case UserAgentTypes.iphone:
                    return "Mozilla/5.0 (iPhone; CPU iPhone OS 9_1 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Version/9.0 Mobile/13B143 Safari/601.1";
                case UserAgentTypes.ipad:
                    return "Mozilla/5.0 (iPad; CPU OS 10_3_1 like Mac OS X) AppleWebKit/603.1.30 (KHTML, like Gecko) Version/10.0 Mobile/14E304 Safari/602.1";
                case UserAgentTypes.chrome:
                    return "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36";
                case UserAgentTypes.edeg:
                    return "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299";
                case UserAgentTypes.wp10:
                    return "Mozilla/5.0 (Windows Phone 10.0; Android 4.2.1; Nokia; Lumia 520) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Mobile Safari/537.36 Edge/12.0";
                case UserAgentTypes.ie11:
                    return "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; .NET4.0C; .NET4.0E; .NET CLR 2.0.50727; .NET CLR 3.0.30729; .NET CLR 3.5.30729; rv:11.0) like Gecko";
                case UserAgentTypes.ie10:
                    return "Mozilla/5.0(compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0)";
                case UserAgentTypes.ie9:
                    return "Mozilla/5.0(compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
                case UserAgentTypes.ie8:
                    return "Mozilla/4.0(compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0)";
                case UserAgentTypes.ie7:
                    return "Mozilla/4.0(compatible; MSIE 7.0; Windows NT 6.0)";
                case UserAgentTypes.ie6:
                    return "Mozilla/4.0(compatible; MSIE 6.0; Windows NT 5.1)";
                case UserAgentTypes.firefox:
                    return "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:52.0) Gecko/20100101 Firefox/52.0";
                case UserAgentTypes.opera:
                    return "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.90 Safari/537.36 OPR/47.0.2631.80";
                default:
                    return "";
            }
        }
        /// <summary>
        /// url查询字符串转字典列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static Dictionary<string, string> QueryStringToDict(string query)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (string q in query.Split("&".ToArray()))
            {
                var s = q.Split("=".ToArray());
                string key = s[0].Replace("?", "").Trim();
                if (s.Length == 2)
                {
                    dict.Add(key, s[1]);
                }
                else
                {
                    dict.Add(key, "");
                }
            }
            return dict;
        }
    }
}