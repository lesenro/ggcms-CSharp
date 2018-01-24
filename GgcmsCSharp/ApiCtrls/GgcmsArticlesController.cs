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
            if (article.ModuleInfo != null)
            {
                ModuleSave(article.Id, article.ModuleInfo);
            }
            return new ResultData
            {
                Code = 0,
                Data = dbHelper.Edit(article.Id, article),
                Msg = ""
            };
        }
        private void ModuleSave(int aid, GgcmsModule module)
        {
            var cols = dbHelper.dbCxt.GgcmsModuleColumns.Where(x => x.Module_Id == module.Id);
            var m = dbHelper.dbCxt.GgcmsModules.Find(module.Id);
            foreach (var col in cols)
            {
                var c = module.Columns.Find(x => x.Id == col.Id);
                if (c != null)
                {
                    col.Value = c.Value;
                }
            }
            using (GgcmsDB db = new GgcmsDB())
            {
                string sql = "SELECT COUNT(*) FROM "+m.TableName+" WHERE Articles_Id= "+aid.ToString();
                //添加
                if (db.Database.SqlQuery<int>(sql).First() == 0)
                {
                    List<string> colsStr = new List<string>();
                    List<string> paramStr = new List<string>();
                    List<object> valus = new List<object>();
                    colsStr.Add("[Articles_Id]");
                    paramStr.Add("Articles_Id");
                    valus.Add(aid);
                    foreach (var col in cols)
                    {
                        colsStr.Add("[" + col.ColName + "]");
                        paramStr.Add("@" + col.ColName);
                        valus.Add(col.Value);
                    }
                    sql = "INSERT INTO ["+m.TableName+"] ( "+ string.Join(",", colsStr) + " ) VALUES (  " + string.Join(",", paramStr) + " )";
                    db.Database.ExecuteSqlCommand(sql, valus);
                }
                //修改
                else
                {
                    List<string> colsStr = new List<string>();
                    List<object> valus = new List<object>();
                    foreach (var col in cols)
                    {
                        colsStr.Add("[" + col.ColName + "] = @" + col.ColName);
                        valus.Add(col.Value);
                    }
                    valus.Add(aid);
                    sql = "UPDATE ["+m.TableName+"] SET " + string.Join(",", colsStr)+ " Where [Articles_Id] = @Articles_Id";
                    db.Database.ExecuteSqlCommand(sql, valus);
                }
            }
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
            UpFileClass.FileSave<GgcmsArticle>(article, article.files);
            article.CreateTime = DateTime.Now;
            updateArticleNumber(article.Category_Id, 1);
            CacheHelper.RemoveAllCache(CacheTypeNames.Categorys);
            article = dbHelper.Add(article);
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