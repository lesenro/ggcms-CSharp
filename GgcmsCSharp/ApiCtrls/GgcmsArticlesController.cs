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
using GgcmsCSharp.Utils;
using System.Web;
using System.Data.Entity;

namespace GgcmsCSharp.ApiCtrls
{
    public class GgcmsArticlesController : ApiBaseController
    {
        // GET: api/GgcmsCategories
        [HttpGet]
        public IHttpActionResult GetList(string query)
        {
            string json = HttpUtility.UrlDecode(query);
            SearchParams sParams = Tools.JsonDeserialize<SearchParams>(json);

            return Ok(GetRecords<GgcmsArticles>(sParams));
        }
        public IHttpActionResult GetGgcmsModuleValue(int aid, int mid)
        {
            return Ok(ExtendModule.GetModuleToDict(aid, mid));
        }
        // GET: api/GgcmsCategories/5
        public IHttpActionResult GetInfo(int id)
        {
            var info = Dbctx.GgcmsArticles.Find(id);
            var list = from r in Dbctx.GgcmsAttachments
                       where r.Articles_Id == id
                       select r;
            info.attachments = list.ToList();
            var pages = Dbctx.GgcmsArticlePages
                .Where(x => x.Article_Id == id);
            info.pagesCount = 1;
            info.pages = new List<GgcmsArticlePages>();
            if (pages.Count() > 0)
            {
                info.pagesCount = pages.Count() + 1;
                info.pages = pages.OrderBy(x => x.OrderId).Select(x => new GgcmsArticlePages
                {
                    Article_Id = x.Article_Id,
                    Id = x.Id,
                    OrderId = x.OrderId,
                    state = EntityState.Detached

                }).ToList();
            }
            return Ok(info);
        }
        /// <summary>
        /// 获取分页内容
        /// </summary>
        /// <param name="id">文章id</param>
        /// <param name="pn">页码</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetPageInfo(int id, int pn)
        {
            var info = Dbctx.GgcmsArticlePages.Where(x => x.Article_Id == id && x.OrderId == pn).FirstOrDefault();
            return Ok(info);
        }
        // PUT: api/GgcmsCategories/5
        public IHttpActionResult Edit(GgcmsArticles info)
        {
            if (Dbctx.GgcmsArticles.Where(x => x.Id == info.Id).Count() == 0)
            {
                return BadRequest("信息不存在");
            }
            //Dbctx.GgcmsArticles.Attach(info);
            //Dbctx.Entry(info).Property("goods_name").IsModified = true;
            var ent = Dbctx.Entry(info);
            ent.State = EntityState.Modified;
            info.pagesCount = info.pages.Where(x => x.state != EntityState.Deleted).Count()+1;
            UpFileClass.FileSave(info, info.files.FindAll(x => x.fileType != 3));
            var old = Dbctx.GgcmsArticles.Find(info.Id);
            if (old.Category_Id != info.Category_Id)
            {
                updateArticleNumber(info.Category_Id, 1);
                updateArticleNumber(old.Category_Id, -1);
                CacheHelper.RemoveAllCache(CacheTypeNames.Categorys);
            }
            var list = Dbctx.GgcmsAttachments.Where(x => x.Articles_Id == info.Id).ToList();
            //附件
            foreach (GgcmsAttachments attach in list)
            {
                var item = info.attachments.Find(x => x.Id == attach.Id);
                if (item == null)
                {
                    Dbctx.GgcmsAttachments.Remove(attach);
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
            foreach (GgcmsAttachments attach in info.attachments)
            {
                if (attach.Id == 0)
                {
                    attach.Articles_Id = info.Id;
                    attach.CreateTime = DateTime.Now;
                    Dbctx.GgcmsAttachments.Add(attach);
                }
            }
            if (info.ModuleInfo != null)
            {
                info.ExtModelId = info.ModuleInfo.Id;
                if (old.ExtModelId > 0 && info.ExtModelId != old.ExtModelId)
                {
                    ExtendModule.Delete(info.Id, old.ExtModelId);
                }
                foreach (var file in info.files.FindAll(x => x.fileType == 3))
                {
                    foreach (var item in info.ModuleInfo.Columns)
                    {
                        if (item.ColName == file.propertyName)
                        {
                            item.Value = UpFileClass.FileSave(file.filePath.ToString(), item.Value.ToString(), (int)file.fileType);
                        }
                    }
                }
                ExtendModule.SaveData(info.Id, info.ModuleInfo);
            }
            else if (old.ExtModelId > 0)
            {
                ExtendModule.Delete(info.Id, old.ExtModelId);
            }
            //分页保存
            if (info.pages.Count > 0)
            {
                foreach (var page in info.pages)
                {
                    if (page.state == EntityState.Added)
                    {
                        UpFileClass.FileSave(page, page.files);
                        page.Article_Id = info.Id;
                        Dbctx.GgcmsArticlePages.Add(page);
                    }
                    else if (page.state == EntityState.Modified)
                    {
                        var pageEnt = Dbctx.Entry(page);
                        pageEnt.State = EntityState.Modified;
                    }
                    else if (page.state == EntityState.Deleted)
                    {
                        var p = Dbctx.GgcmsArticlePages.Find(page.Id);
                        if (p != null)
                        {
                            Dbctx.GgcmsArticlePages.Remove(p);
                        }
                    }
                    //只更新分页信息
                    else if (page.state == EntityState.Unchanged)
                    {
                        Dbctx.GgcmsArticlePages.Attach(page);
                        Dbctx.Entry(info).Property("OrderId").IsModified = true;
                    }
                }
            }
            Dbctx.SaveChanges();
            ClearCache();
            return Ok(info);
        }

        // POST: api/GgcmsCategories
        public IHttpActionResult Add(GgcmsArticles info)
        {


            //提交除附加模型外的文件-标题图，内容中的图
            UpFileClass.FileSave(info, info.files.FindAll(x => x.fileType != 3));
            info.CreateTime = DateTime.Now;
            updateArticleNumber(info.Category_Id, 1);
            CacheHelper.RemoveAllCache(CacheTypeNames.Categorys);
            if (info.ModuleInfo != null && info.ModuleInfo.Id > 0)
            {
                info.ExtModelId = info.ModuleInfo.Id;
            }
            info.pagesCount = info.pages.Count + 1;
            var result = Dbctx.GgcmsArticles.Add(info);
            Dbctx.SaveChanges();
            //附件
            using (GgcmsDB db = new GgcmsDB())
            {
                foreach (GgcmsAttachments attach in info.attachments)
                {
                    attach.Articles_Id = info.Id;
                    attach.CreateTime = DateTime.Now;
                    db.GgcmsAttachments.Add(attach);
                }
                db.SaveChanges();
            }
            //数据模型
            if (info.ModuleInfo != null && info.ModuleInfo.Id > 0)
            {
                foreach (var file in info.files.FindAll(x => x.fileType == 3))
                {
                    foreach (var item in info.ModuleInfo.Columns)
                    {
                        if (item.ColName == file.propertyName)
                        {
                            item.Value = UpFileClass.FileSave(file.filePath.ToString(), item.Value.ToString(), (int)file.fileType);
                        }
                    }
                }
                ExtendModule.SaveData(info.Id, info.ModuleInfo);
            }

            //分页保存
            if (info.pages.Count > 0)
            {
                foreach (var page in info.pages)
                {
                    UpFileClass.FileSave(page, page.files);
                    page.Article_Id = info.Id;
                    Dbctx.GgcmsArticlePages.Add(page);
                }
                Dbctx.SaveChanges();
            }
            ClearCache();
            return Ok(result);

        }

        // DELETE: api/GgcmsCategories/5
        public IHttpActionResult Delete(int id)
        {

            GgcmsArticles info = Dbctx.GgcmsArticles.Find(id);
            if (info == null)
            {
                return BadRequest("信息不存在");
            }

            //删除关联模型数据
            var category = dbHelper.Categories(Math.Abs( info.Category_Id));
            if (category.ExtModelId > 0)
            {
                ExtendModule.Delete(info.Id, category.ExtModelId);
            }
            updateArticleNumber(info.Category_Id, -1);

            Dbctx.GgcmsArticles.Remove(info);
            //删除关联附件
            Dbctx.GgcmsAttachments.RemoveRange(Dbctx.GgcmsAttachments.Where(x => x.Articles_Id == info.Id));
            //删除关联分页
            Dbctx.GgcmsArticlePages.RemoveRange(Dbctx.GgcmsArticlePages.Where(x => x.Article_Id == info.Id));
            Dbctx.SaveChanges();
            ClearCache();
            CacheHelper.RemoveAllCache(CacheTypeNames.Categorys);
            return Ok(info);
        }
        [HttpPost]
        public IHttpActionResult MultDelete(int[] ids)
        {
            var query = Dbctx.GgcmsArticles.Where(x => ids.Contains(x.Id));
            foreach (GgcmsArticles item in query.ToList())
            {

                updateArticleNumber(item.Category_Id, -1);
                //删除关联附件
                var attalist = Dbctx.GgcmsAttachments.Where(x => x.Articles_Id == item.Id);
                Dbctx.GgcmsAttachments.RemoveRange(attalist);
                //删除关联模型数据
                var category = dbHelper.Categories(Math.Abs(item.Category_Id));
                if (category.ExtModelId > 0)
                {
                    ExtendModule.Delete(item.Id, category.ExtModelId);
                }
                //删除关联分页
                Dbctx.GgcmsArticlePages.RemoveRange(Dbctx.GgcmsArticlePages.Where(x => x.Article_Id == item.Id));
            }
            Dbctx.GgcmsArticles.RemoveRange(query);
            int c = Dbctx.SaveChanges();
            ClearCache();
            CacheHelper.RemoveAllCache(CacheTypeNames.Categorys);
            return Ok(c);
        }


        public IHttpActionResult Exists(int id)
        {
            return Ok(Dbctx.GgcmsArticles.Where(x => x.Id == id).Count() > 0);

        }
        private void updateArticleNumber(int id, int num)
        {

            try
            {
                using (GgcmsDB db = new GgcmsDB())
                {
                    GgcmsCategories cinfo = db.GgcmsCategories.Find(id);
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