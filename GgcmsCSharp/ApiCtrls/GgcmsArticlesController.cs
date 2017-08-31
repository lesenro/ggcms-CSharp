using GgcmsCSharp.Models;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System;
using GgcmsCSharp.Controllers;
using System.Collections.Generic;
using System.Linq;

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
            ResultData rdata= dbtool.GetById(id);
            if (rdata.Code == 0)
            {
                GgcmsArticle artInfo = (GgcmsArticle)rdata.Data;
                GgcmsDB db = new GgcmsDB();
                var list = from r in db.GgcmsAttachments
                           where r.Articles_Id == artInfo.Id
                           select r;
                artInfo.attachments = list.ToList();
                rdata.Data = artInfo;
            }
            return rdata;
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
                var list = db.GgcmsAttachments.Where(x => x.Articles_Id == article.Id).ToList();
                foreach (GgcmsAttachment attach in list)
                {
                    var item = article.attachments.Find(x => x.Id == attach.Id);
                    if (item == null)
                    {
                        db.GgcmsAttachments.Remove(attach);
                    }
                    else
                    {
                        attach.AttaUrl = item.AttaUrl;
                        attach.AttaTitle = item.AttaTitle;
                        attach.Describe = item.Describe;
                        attach.CreateTime = DateTime.Now;
                        attach.RealName = item.RealName;
                    }
                }
                foreach (GgcmsAttachment attach in article.attachments)
                {
                    if (attach.Id == 0)
                    {
                        attach.Articles_Id = article.Id;
                        attach.CreateTime = DateTime.Now;
                        db.GgcmsAttachments.Add(attach);
                    }
                }
                db.SaveChanges();
            }
            return dbtool.Edit(article.Id, article);
        }

        // POST: api/GgcmsCategories
        public ResultData Add(GgcmsArticle article)
        {
            ResultData result;
            if (!ModelState.IsValid)
            {
                result = new ResultData
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
            result = dbtool.Add(article);
            using (GgcmsDB db = new GgcmsDB())
            {
                foreach (GgcmsAttachment attach in article.attachments)
                {
                    attach.Articles_Id = article.Id;
                    attach.CreateTime = DateTime.Now;
                    db.GgcmsAttachments.Add(attach);
                }
                db.SaveChanges();
            }

            return result;
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
                using (GgcmsDB db = new GgcmsDB())
                {
                    db.GgcmsArticles.Remove(info);
                    var list = db.GgcmsAttachments.Where(x => x.Articles_Id == info.Id).ToList();
                    foreach (GgcmsAttachment attach in list)
                    {
                        db.GgcmsAttachments.Remove(attach);
                    }
                    db.SaveChanges();
                    return new ResultData
                    {
                        Code = 0,
                        Msg = "ok"
                    };
                }
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
            if (disposing && dbtool != null)
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