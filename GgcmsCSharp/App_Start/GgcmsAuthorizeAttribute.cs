using GgcmsCSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace GgcmsCSharp
{
    public class GgcmsAuthorizeAttribute : AuthorizeAttribute
    {
        private string power;
        private List<string> whiteList = new List<string> { "PostLogin$","Webapi/", "ggcmsdatatools/dbinit" };
        public GgcmsAuthorizeAttribute(string power)
        {
            this.power = power;
        }
        public GgcmsAuthorizeAttribute()
        {
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            try
            {
                string url = actionContext.Request.RequestUri.AbsoluteUri;
                string path = actionContext.Request.RequestUri.AbsolutePath;
                foreach(string p in whiteList)
                {
                    if(Regex.IsMatch(url,p, RegexOptions.IgnoreCase))
                    {
                        return true;
                    }
                    
                }
                var session = HttpContext.Current.Session;
                string sessionKey = SystemEnums.login_user.ToString();
                if (session != null)
                {
                    if (session[sessionKey] != null)
                    {
                        GgcmsMembers m = session[sessionKey] as GgcmsMembers;
                        if (m.Id > 0)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}