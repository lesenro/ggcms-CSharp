using GgcmsCSharp.Controllers;
using GgcmsCSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public IHttpActionResult articles()
        {
            var reqParams = InitRequestParams<GgcmsArticle>();
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
        public IHttpActionResult views(int id)
        {
            var reqParams = InitRequestParams();
            return Ok(ExtendModule.ViewArticles(id, reqParams));
        }
        [HttpGet]
        public IHttpActionResult addview(int id)
        {
            var info = dbHelper.dbCxt.GgcmsArticles.Find(id);
            if (info != null)
            {
                info.Hits++;
                dbHelper.dbCxt.SaveChanges();
                return Ok(info.Hits);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet]
        public IHttpActionResult friendlinks()
        {
            var reqParams = InitRequestParams<GgcmsFriendLink>();
            return Ok(dataHelper.FriendLinks(reqParams.query, reqParams.columns, reqParams.offset, reqParams.offset, reqParams.sortby, reqParams.order));
        }
        [HttpGet]
        public IHttpActionResult adverts()
        {
            var reqParams = InitRequestParams<GgcmsAdverts>();
            return Ok(dataHelper.Adverts(reqParams.query, reqParams.columns, reqParams.offset, reqParams.offset, reqParams.sortby, reqParams.order));
        }
    }
}
