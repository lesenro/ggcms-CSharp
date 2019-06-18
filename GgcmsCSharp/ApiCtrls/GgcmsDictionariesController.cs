using GgcmsCSharp.Models;
using GgcmsCSharp.Utils;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;

namespace GgcmsCSharp.ApiCtrls
{
    public class GgcmsDictionariesController : ApiBaseController
    {

        // GET: api/GgcmsCategories
        [HttpGet]
        public IHttpActionResult GetList(string query)
        {
            string json = HttpUtility.UrlDecode(query);
            SearchParams sParams = Tools.JsonDeserialize<SearchParams>(json);

            return Ok(GetRecords<GgcmsDictionaries>(sParams));
        }

        // GET: api/GgcmsCategories/5
        public IHttpActionResult GetInfo(int id)
        {
            return Ok(Dbctx.GgcmsDictionaries.Find(id));
        }

        // PUT: api/GgcmsCategories/5
        public IHttpActionResult Edit(GgcmsDictionaries info)
        {

            if (Dbctx.GgcmsDictionaries.Where(x => x.Id == info.Id).Count() == 0)
            {
                return BadRequest("信息不存在");
            }
            //Dbctx.GgcmsDictionaries.Attach(info);
            //Dbctx.Entry(info).Property("goods_name").IsModified = true;
            var ent = Dbctx.Entry(info);
            ent.State = EntityState.Modified;

            Dbctx.SaveChanges();
            ClearCache();
            return Ok(info);
        }

        // POST: api/GgcmsCategories
        public IHttpActionResult Add(GgcmsDictionaries info)
        {
            var result = Dbctx.GgcmsDictionaries.Add(info);
            Dbctx.SaveChanges();
            ClearCache();
            return Ok(result);
        }

        // DELETE: api/GgcmsCategories/5
        public IHttpActionResult Delete(int id)
        {
            GgcmsDictionaries oldinfo = Dbctx.GgcmsDictionaries.Find(id);
            if (oldinfo == null)
            {
                return BadRequest("信息不存在");
            }

            Dbctx.GgcmsDictionaries.Remove(oldinfo);
            Dbctx.SaveChanges();
            ClearCache();
            return Ok(oldinfo);
        }
        [HttpPost]
        public IHttpActionResult MultDelete(int[] ids)
        {
            var query = Dbctx.GgcmsDictionaries.Where(x => ids.Contains(x.Id));

            Dbctx.GgcmsDictionaries.RemoveRange(query);
            int c = Dbctx.SaveChanges();
            ClearCache();
            return Ok(c);

        }
       

        public IHttpActionResult Exists(int id)
        {
            return Ok(Dbctx.GgcmsDictionaries.Where(x => x.Id == id).Count() > 0);
        }

        public IHttpActionResult SettingsSave(dynamic data)
        {

            try
            {
                foreach (var file in data.files)
                {
                    foreach (var item in data.list)
                    {
                        if (item.DictKey.ToString() == file.propertyName.ToString())
                        {
                            item.DictValue = UpFileClass.FileSave(file.filePath.ToString(), item.DictValue.ToString(), (int)file.fileType);
                        }
                    }
                }
                Dbctx.GgcmsDictionaries
                   .ToList()
                   .ForEach(x =>
                   {
                       foreach (var item in data.list)
                       {
                           if (x.Id == (int)item.Id)
                           {
                               x.DictValue = item.DictValue.ToString();
                           }
                       }
                   });
                Dbctx.SaveChanges();
                CacheHelper.RemoveAllCache(CacheTypeNames.SysConfigs.ToString());
                return Ok(data.list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}