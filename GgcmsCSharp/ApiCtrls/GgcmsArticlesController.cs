using GgcmsCSharp.Models;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System;
using GgcmsCSharp.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Text;

namespace GgcmsCSharp.ApiCtrls
{
    public class GgcmsArticlesController : ApiBaseController
    {
        // GET: api/GgcmsCategories
        [HttpGet]
        public ResultData GetList()
        {
            var reqParams = InitRequestParams<GgcmsArticle>();
            var result = dbHelper.GetList<GgcmsArticle>(reqParams);
            return new ResultData
            {
                Code = 0,
                Data = result,
                Msg = ""
            };
        }
        public ResultData GetGgcmsModuleValue(int aid, int mid)
        {
            return new ResultData
            {
                Code = 0,
                Data = ExtendModule.GetModuleToDict(aid, mid),
                Msg = ""
            };
        }
        // GET: api/GgcmsCategories/5
        public ResultData GetInfo(int id)
        {
            var result = dbHelper.GetById<GgcmsArticle>(id);
            if (result !=null)
            {
                using (GgcmsDB db = new GgcmsDB())
                {
                    var list = from r in db.GgcmsAttachments
                               where r.Articles_Id == result.Id
                               select r;
                    result.attachments = list.ToList();
                }
            }
            return new ResultData
            {
                Code = 0,
                Data = result,
                Msg = ""
            };
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
            GgcmsArticle old;
            using (GgcmsDB db = new GgcmsDB())
            {
                UpFileClass.FileSave(article, article.files);
                old = db.GgcmsArticles.Find(article.Id);
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
            if (article.ModuleInfo != null)
            {
                article.ExtModelId = article.ModuleInfo.Id;
                if (old.ExtModelId > 0&&article.ExtModelId!=old.ExtModelId)
                {
                    ExtendModule.Delete(article.Id, old.ExtModelId);
                }
                ExtendModule.SaveData(article.Id, article.ModuleInfo);
            }
            else if(old.ExtModelId>0)
            {
                ExtendModule.Delete(article.Id, old.ExtModelId);
            }
            return new ResultData
            {
                Code = 0,
                Data = dbHelper.Edit(article.Id, article),
                Msg = ""
            };
        }

        // POST: api/GgcmsCategories
        public ResultData Add(GgcmsArticle article)
        {
            if (!ModelState.IsValid)
            {
                return new ResultData
                {
                    Code = 3,
                    Msg = "",
                    Data = BadRequest(ModelState)
                }; 
            }
            UpFileClass.FileSave(article, article.files);
            article.CreateTime = DateTime.Now;
            updateArticleNumber(article.Category_Id, 1);
            CacheHelper.RemoveAllCache(CacheTypeNames.Categorys);
            article = dbHelper.Add(article);
            if (article.ModuleInfo != null)
            {
                article.ExtModelId = article.ModuleInfo.Id;
                ExtendModule.SaveData(article.Id, article.ModuleInfo);
            }
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

            return new ResultData
            {
                Code = 0,
                Msg = "",
                Data = article
            }; ;
        }

        // DELETE: api/GgcmsCategories/5
        public ResultData Delete(int id)
        {
            GgcmsArticle info = dbHelper.dbCxt.GgcmsArticles.Find(id);
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
            var reqParams = InitRequestParams<GgcmsArticle>();
            var articleList = dbHelper.GetList<GgcmsArticle>(reqParams);
            using (GgcmsDB db = new GgcmsDB())
            {
                foreach (var item in articleList.List)
                {
                    GgcmsArticle info = item as GgcmsArticle;

                    if (info != null)
                    {
                        updateArticleNumber(info.Category_Id, -1);
                    }
                    var attalist = db.GgcmsAttachments.Where(x => x.Articles_Id == info.Id);
                    db.GgcmsAttachments.RemoveRange(attalist);
                }
                db.SaveChanges();
            }
            CacheHelper.RemoveAllCache(CacheTypeNames.Categorys);
            return new ResultData
            {
                Code = 0,
                Msg = "",
                Data = dbHelper.MultDelete<GgcmsArticle>(reqParams)
            };
        }


        public ResultData Exists(int id)
        {
            return new ResultData
            {
                Code = 0,
                Msg = "",
                Data = dbHelper.Exists<GgcmsArticle>(id)
            };
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