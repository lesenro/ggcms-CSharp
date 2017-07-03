using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.WebHost;
using System.Web.Routing;
using System.Web.SessionState;
using GgcmsCSharp.ApiCtrls;
using System.Configuration;

namespace GgcmsCSharp
{
    public static class WebApiConfig
    {
        public class SessionControllerHandler : HttpControllerHandler, IRequiresSessionState
        {
            public SessionControllerHandler(RouteData routeData)
                : base(routeData)
            { }
        }

        public class SessionHttpControllerRouteHandler : HttpControllerRouteHandler
        {
            protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
            {
                return new SessionControllerHandler(requestContext.RouteData);
            }
        }
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
            var httpControllerRouteHandler = typeof(HttpControllerRouteHandler).GetField("_instance", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            if (httpControllerRouteHandler != null)
            {
                httpControllerRouteHandler.SetValue(null,
                    new Lazy<HttpControllerRouteHandler>(() => new SessionHttpControllerRouteHandler(), true));
            }
            // Web API 路由
            config.MapHttpAttributeRoutes();
            //注册全局Filter
            config.Filters.Add(new GgcmsAuthorizeAttribute());
            string adminApi = ConfigurationManager.AppSettings["AdminApi"].ToString();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: adminApi+"/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
