using GgcmsCSharp.Controllers;
using GgcmsCSharp.Models;
using GgcmsCSharp.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.SessionState;

namespace GgcmsCSharp.ApiCtrls
{
    public class ApiBaseController : ApiController
    {
        public GgcmsDB Dbctx { get; set; }
        public DataHelper dbHelper { get; set; }
        public HttpContext Context { get; set; }
        public HttpSessionState Session { get; set; }
        public HttpServerUtility Server { get; set; }
        public HttpRequest httpRequest { get; set; }
        public HttpResponse httpResponse { get; set; }

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            Context = HttpContext.Current;
            Session = Context.Session;
            Server = Context.Server;
            httpRequest = Context.Request;
            httpResponse = Context.Response;

            base.Initialize(controllerContext);
            Context = HttpContext.Current;
            Session = Context.Session;
            Server = Context.Server;
            httpRequest = Context.Request;
            httpResponse = Context.Response;
            Dbctx = new GgcmsDB();
            dbHelper = new DataHelper();
        }

        [NonAction]
        public GgcmsMembers GetLoginUser()
        {
            //没有登录
            if (Session[SystemEnums.login_user.ToString()] == null)
            {
                return null;
            }

            var user = Session[SystemEnums.login_user.ToString()] as GgcmsMembers;

            return user;
        }
        [NonAction]
        public List<GgcmsPowers> GetUserPowers()
        {
            if (Session[SystemEnums.user_powers.ToString()] == null)
            {
                return null;
            }

            var my_powers = Session[SystemEnums.user_powers.ToString()] as List<GgcmsPowers>;

            return my_powers;
        }
        [NonAction]
        public void ClearCache()
        {
            string cachePath = Request.RequestUri.LocalPath;
            cachePath = cachePath.Substring(0, cachePath.LastIndexOf("/"));
            CacheHelper.RemoveAllCache(cachePath);
        }
        [NonAction]
        public void ClearCache(string key)
        {
            string cachePath = Request.RequestUri.LocalPath;
            cachePath = cachePath.Substring(0, cachePath.LastIndexOf("/"));
            var route = ActionContext.RequestContext.RouteData;
            string ctrl_name = route.Values["controller"].ToString();
            cachePath = cachePath.Replace(ctrl_name, key);
            //清理门票API的缓存
            CacheHelper.RemoveAllCache(cachePath);
        }
        [NonAction]
        public SearchResult<T> GetRecords<T>(SearchParams sParams) where T : class
        {
            return dbHelper.GetRecords<T>(sParams);
        }
    }
}
