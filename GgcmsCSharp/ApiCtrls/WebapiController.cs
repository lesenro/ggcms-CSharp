using GgcmsCSharp.Controllers;
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

namespace GgcmsCSharp.ApiCtrls
{
    public class WebapiController : ApiBaseController
    {
        private DataHelper dataHelper;
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            dataHelper = new DataHelper();
        }
        [HttpGet]
        public IHttpActionResult articles(string query)
        {
            string json = HttpUtility.UrlDecode(query);
            SearchParams reqParams = Tools.JsonDeserialize<SearchParams>(json);
            return Ok(dataHelper.Articles(reqParams));
        }
        [HttpGet]
        public IHttpActionResult categories()
        {
            return Ok(dataHelper.Categories());
        }
        [HttpGet]
        public IHttpActionResult categories(int id)
        {
            return Ok(dataHelper.Categories(id));
        }

        [HttpGet]
        public IHttpActionResult articles(int id)
        {
            return Ok(dataHelper.Article(id));
        }
        [HttpGet]
        public IHttpActionResult views(int id, string query)
        {
            string json = HttpUtility.UrlDecode(query);
            SearchParams reqParams = Tools.JsonDeserialize<SearchParams>(json);
            return Ok(ExtendModule.ViewArticles(id, reqParams));
        }
        [HttpGet]
        public IHttpActionResult addview(int id)
        {
            var info = Dbctx.GgcmsArticles.Find(id);
            if (info != null)
            {
                info.Hits++;
                Dbctx.SaveChanges();
                return Ok(info.Hits);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet]
        public IHttpActionResult friendlinks(string query)
        {
            string json = HttpUtility.UrlDecode(query);
            SearchParams reqParams = Tools.JsonDeserialize<SearchParams>(json);
            return Ok(dataHelper.FriendLinks(reqParams.QueryString, reqParams.Columns, reqParams.PageNum, reqParams.PageSize, reqParams.OrderBy));
        }
        [HttpGet]
        public IHttpActionResult adverts(string query)
        {
            string json = HttpUtility.UrlDecode(query);
            SearchParams reqParams = Tools.JsonDeserialize<SearchParams>(json);
            return Ok(dataHelper.Adverts(reqParams.QueryString, reqParams.Columns, reqParams.PageNum, reqParams.PageSize, reqParams.OrderBy));
        }

        [HttpGet]
        public IHttpActionResult configs()
        {
            return Ok(dataHelper.SysConfigs());
        }
        [HttpGet]
        public IHttpActionResult dictionary(string query)
        {
            string json = HttpUtility.UrlDecode(query);
            SearchParams reqParams = Tools.JsonDeserialize<SearchParams>(json);
            if (string.IsNullOrWhiteSpace(reqParams.OrderBy))
            {
                reqParams.OrderBy = "OrderID asc";
            }
            return Ok(GetRecords<GgcmsDictionaries>(reqParams));
        }
    }
}
