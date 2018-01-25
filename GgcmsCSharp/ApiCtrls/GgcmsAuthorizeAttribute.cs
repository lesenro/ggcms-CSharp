using GgcmsCSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace GgcmsCSharp.ApiCtrls
{
    public class GgcmsAuthorizeAttribute : AuthorizeAttribute
    {
        private string power;
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
                if (url.EndsWith("PostLogin")||path.Contains("Webapi/"))
                {
                    return true;
                }
                var session = HttpContext.Current.Session;
                
                if (session != null)
                {
                    if (session["ggcms_loginUser"] != null)
                    {
                        GgcmsMember m = session["ggcms_loginUser"] as GgcmsMember;
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