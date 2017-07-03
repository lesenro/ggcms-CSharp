using GgcmsCSharp.Models;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System;
using GgcmsCSharp.Controllers;
using System.Collections.Generic;

namespace GgcmsCSharp.ApiCtrls
{
    public class GgcmsArticlesController : ApiController
    {
        private dbTools<GgcmsArticle> dbtool;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            this.dbtool = new dbTools<GgcmsArticle>(Request);
        }
        // GET: api/GgcmsCategories
        [HttpGet]
        public ResultData GetList()
        {
            return dbtool.GetList();
        }

        // GET: api/GgcmsCategories/5
        public ResultData GetInfo(int id)
        {
            return dbtool.GetById(id);
        }

        // PUT: api/GgcmsCategories/5
        public ResultData Edit(GgcmsArticle article)
        {

            if (!ModelState.IsValid)
            {
                ResultData result = new ResultData
                {
                    Code = 3,
                    Msg = "",
                    Data = BadRequest(ModelState)
                };
                return result;
            }
            using (GgcmsDB db = new GgcmsDB())
            {
                UpFileClass.FileSave<GgcmsArticle>(article, article.files);
                GgcmsArticle old = db.GgcmsArticles.Find(article.Id);
                if (old.Category_Id != article.Category_Id)
                {
                    updateArticleNumber(article.Category_Id, 1);
                    updateArticleNumber(old.Category_Id, -1);
                    CacheHelper.RemoveAllCache(CacheTypeNames.Categorys);
                }
            }
            return dbtool.Edit(article.Id, article);
        }

        // POST: api/GgcmsCategories
        public ResultData Add(GgcmsArticle article)
        {
            if (!ModelState.IsValid)
            {
                ResultData result = new ResultData
                {
                    Code = 3,
                    Msg = "",
                    Data = BadRequest(ModelState)
                };
                return result;
            }
            UpFileClass.FileSave<GgcmsArticle>(article, article.files);

            article.CreateTime = DateTime.Now;
            updateArticleNumber(article.Category_Id, 1);
            CacheHelper.RemoveAllCache(CacheTypeNames.Categorys);
            return dbtool.Add(article);
        }

        // DELETE: api/GgcmsCategories/5
        public ResultData Delete(int id)
        {
            GgcmsArticle info = dbtool.db.GgcmsArticles.Find(id);
            if (info == null)
            {
                return new ResultData
                {
                    Code = 1,
                    Msg = "not found "
                };
            }
            updateArticleNumber(info.Category_Id, -1);
            CacheHelper.RemoveAllCache(CacheTypeNames.Categorys);
            try
            {
                dbtool.db.GgcmsArticles.Remove(info);
                dbtool.db.SaveChanges();
                return new ResultData
                {
                    Code = 0,
                    Msg = "ok"
                };
            }
            catch (Exception ex)
            {
                return new ResultData
                {
                    Code = 2,
                    Msg = ex.Message,
                    Data = ex
                };
            }
        }
        [HttpGet]
        public ResultData MultDelete()
        {
            string query = dbtool.DecodeOutputString(Request.GetQueryString("query"));
            query = dbtool.getQuery(query);
            List<int> ids = new List<int>();
            foreach (object val in dbtool.reqParams.queryParams)
            {
                int id = (int)val;
                GgcmsArticle info = dbtool.db.GgcmsArticles.Find(id);
                if (info != null)
                {
                    updateArticleNumber(info.Category_Id, -1);
                }
            }
            CacheHelper.RemoveAllCache(CacheTypeNames.Categorys);
            return dbtool.MultDelete();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbtool.Dispose(true);
            }
            base.Dispose(disposing);
        }

        public ResultData Exists(int id)
        {
            return dbtool.Exists(id);
        }
        private void updateArticleNumber(int id, int num)
        {

            try
            {
                using (GgcmsDB db = new GgcmsDB())
                {
                    GgcmsCategory cinfo = db.GgcmsCategories.Find(id);
                    if (cinfo == null)
                    {
                        return;
                    }
                    if (cinfo.ParentId > 0)
                    {
                        updateArticleNumber(cinfo.ParentId, num);
                    }
                    cinfo.ArticleTotal = cinfo.ArticleTotal + num;
                    db.SaveChanges();
                }
            }
            catch { }
        }
    }
}