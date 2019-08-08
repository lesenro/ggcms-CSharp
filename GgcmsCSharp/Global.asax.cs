using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using System.Web.Routing;
using GgcmsCSharp.Utils;
using System.Configuration;

namespace GgcmsCSharp
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        ScheduleTask scheduleTask;
        protected void Application_Start()
        {
            //����֧��
            GlobalConfiguration.Configuration.EnableCors(new EnableCorsAttribute("*", "*", "*") { SupportsCredentials = true });
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            scheduleTask = new ScheduleTask(this.Context);
            scheduleTask.Start();
        }

        protected void Application_End(object sender, EventArgs e)
        {
            scheduleTask.Stop();

            string Prefix = ConfigurationManager.AppSettings["ServerBaseUrl"].ToString();
            //����Ĵ����ǹؼ����ɽ��IISӦ�ó�����Զ����յ�����  
            System.Threading.Thread.Sleep(1000);
            HttpClient client = new HttpClient();
            client.DownloadString(Prefix);

        }
    }
}
