using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Configuration;

namespace GgcmsCSharp
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码
            string corsurl = ConfigurationManager.AppSettings["CorsUrl"].ToString();
            if (!string.IsNullOrEmpty(corsurl))
            {
                GlobalConfiguration.Configuration.EnableCors(new EnableCorsAttribute(corsurl, "*", "*") { SupportsCredentials = true });
            }
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

    }
}