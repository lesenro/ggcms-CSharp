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
            var info= Dbctx.GgcmsArticles.Find(id);
            var list = from r in Dbctx.GgcmsAttachments
                       where r.Articles_Id == id
                       select r;
            info.attachments = list.ToList();
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

            UpFileClass.FileSave(info, info.files.FindAll(x => x.fileType != 3));
            var old = Dbctx.GgcmsArticles.Find(info.Id);
            if (old.Category_Id != info.Category_Id)
            {
                updateArticleNumber(info.Category_Id, 1);
                updateArticleNumber(old.Category_Id, -1);
                CacheHelper.RemoveAllCache(CacheTypeNames.Categorys);
            }
            var list = Dbctx.GgcmsAttachments.Where(x => x.Articles_Id == info.Id).ToList();
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
           var result = Dbctx.GgcmsArticles.Add(info);
            if (info.ModuleInfo != null && info.ModuleInfo.Id >0)
            {
                foreach (var file in info.files.FindAll(x=>x.fileType == 3))
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

            Dbctx.SaveChanges();
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
            updateArticleNumber(info.Category_Id, -1);

            Dbctx.GgcmsArticles.Remove(info);
            var list = Dbctx.GgcmsAttachments.Where(x => x.Articles_Id == info.Id).ToList();
            foreach (GgcmsAttachments attach in list)
            {
                Dbctx.GgcmsAttachments.Remove(attach);
            }
            Dbctx.SaveChanges();
            ClearCache();
            return Ok(info);
        }
        [HttpPost]
        public IHttpActionResult MultDelete(int[] ids)
        {
            var query = Dbctx.GgcmsArticles.Where(x => ids.Contains(x.Id));
            foreach (GgcmsArticles item in query.ToList())
            {
                updateArticleNumber(item.Category_Id, -1);
                var attalist = Dbctx.GgcmsAttachments.Where(x => x.Articles_Id == item.Id);
                Dbctx.GgcmsAttachments.RemoveRange(attalist);
            }
            Dbctx.GgcmsArticles.RemoveRange(query);
            int c = Dbctx.SaveChanges();
            ClearCache();
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