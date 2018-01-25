using GgcmsCSharp.Models;
using GgcmsCSharp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public DataBaseHelper<GgcmsDB> dbHelper { get; set; }
        public HttpContext Context { get; set; }
        public HttpSessionState Session { get; set; }
        public HttpServerUtility Server { get; set; }
        public HttpRequest httpRequest { get; set; }
        public HttpResponse httpResponse { get; set; }

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            dbHelper = new DataBaseHelper<GgcmsDB>(new GgcmsDB());
            Context = HttpContext.Current;
            Session = Context.Session;
            Server = Context.Server;
            httpRequest = Context.Request;
            httpResponse = Context.Response;
        }

        [NonAction]
        public RequestParams InitRequestParams<T>() where T : class
        {
            string columns = Request.GetQueryString("columns") ?? "";
            int limit = Tools.ConvertType<int>(Request.GetQueryString("limit"), 10);
            int offset = Tools.ConvertType<int>(Request.GetQueryString("offset"), 0);
            int pagenum = Tools.ConvertType<int>(Request.GetQueryString("pagenum"), 1);
            string order = Request.GetQueryString("order") ?? "";
            string sortby = Request.GetQueryString("sortby") ?? "";
            string query = Request.GetQueryString("query") ?? "";
            return RequestParams.GetRequestParams<GgcmsDB, T>(columns, limit, offset, pagenum, order, sortby, query);
        }
        [NonAction]
        public RequestParams InitRequestParams()
        {
            string columns = Request.GetQueryString("columns") ?? "";
            int limit = Tools.ConvertType<int>(Request.GetQueryString("limit"), 10);
            int offset = Tools.ConvertType<int>(Request.GetQueryString("offset"), 0);
            int pagenum = Tools.ConvertType<int>(Request.GetQueryString("pagenum"), 1);
            string order = Request.GetQueryString("order") ?? "";
            string sortby = Request.GetQueryString("sortby") ?? "";
            string query = Request.GetQueryString("query") ?? "";
            return RequestParams.AdoSqlParams<GgcmsDB>(columns, limit, offset, pagenum, order, sortby, query);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && dbHelper != null)
            {
                dbHelper.Dispose(true);
            }
            base.Dispose(disposing);
        }
    }
}
