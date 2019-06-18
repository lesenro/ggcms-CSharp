using GgcmsCSharp.Models;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Linq;
using System;
using System.Web;
using GgcmsCSharp.Utils;
using System.Data.Entity;

namespace GgcmsCSharp.ApiCtrls
{
    public class GgcmsCategoriesController : ApiBaseController
    {

        // GET: api/GgcmsCategories
        [HttpGet]
        [WebApiCache(20)]
        public IHttpActionResult GetList(string query)
        {
            string json = HttpUtility.UrlDecode(query);
            SearchParams sParams = Tools.JsonDeserialize<SearchParams>(json);

            return Ok(GetRecords<GgcmsCategories>(sParams));
        }

        // GET: api/GgcmsCategories/5
        [HttpGet]
        [WebApiCache(20)]
        public IHttpActionResult GetInfo(int id)
        {
            return Ok(Dbctx.GgcmsCategories.Find(id));
        }

        // PUT: api/GgcmsCategories/5
        public IHttpActionResult Edit(GgcmsCategories info)
        {
            if (Dbctx.GgcmsCategories.Where(x => x.Id == info.Id).Count() == 0)
            {
                return BadRequest("信息不存在");
            }
            info.RouteKey = HttpUtility.UrlEncode(info.RouteKey);
            UpFileClass.FileSave(info, info.files);

            //Dbctx.GgcmsCategories.Attach(info);
            //Dbctx.Entry(info).Property("goods_name").IsModified = true;
            var ent = Dbctx.Entry(info);
            ent.State = EntityState.Modified;
            Dbctx.SaveChanges();
            ClearCache();
            return Ok(info);


        }

        // POST: api/GgcmsCategories
        public IHttpActionResult Add(GgcmsCategories info)
        {
            var result = Dbctx.GgcmsCategories.Add(info);
            UpFileClass.FileSave(info, info.files);
            CacheHelper.RemoveAllCache(CacheTypeNames.Categorys);

            Dbctx.SaveChanges();
            ClearCache();
            return Ok(result);
        }
        public IHttpActionResult CategorySortSave(dynamic[] list)
        {
            try {
                Dbctx.GgcmsCategories
                   .ToList()
                   .ForEach(x => {
                       foreach (var item in list)
                       {
                           if (x.Id == (int)item.Id)
                           {
                               x.OrderId = (int)item.OrderId;
                               x.ParentId = (int)item.ParentId;
                           }
                       }
                   });

                int c = Dbctx.SaveChanges();
                ClearCache();
                CacheHelper.RemoveAllCache(CacheTypeNames.Categorys);
                return Ok(c);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        // DELETE: api/GgcmsCategories/5
        [HttpGet]
        public IHttpActionResult Delete(int id)
        {

            GgcmsCategories oldinfo = Dbctx.GgcmsCategories.Find(id);
            if (oldinfo == null)
            {
                return BadRequest("信息不存在");
            }

            //List<int> idlist = GetDeleteIds(oldinfo.ticket_key);

            //var query = Dbctx.ticket_information.Where(x => idlist.Contains(x.id));
            Dbctx.GgcmsCategories.Remove(oldinfo);
            Dbctx.SaveChanges();
            ClearCache();
            return Ok(oldinfo);
        }
        [HttpPost]
        public IHttpActionResult MultDelete(int[] ids)
        {
            var query = Dbctx.GgcmsCategories.Where(x => ids.Contains(x.Id));

            Dbctx.GgcmsCategories.RemoveRange(query);
            int c = Dbctx.SaveChanges();
            ClearCache();
            return Ok(c);
        }


        public IHttpActionResult Exists(int id)
        {
            return Ok(Dbctx.GgcmsCategories.Where(x => x.Id == id).Count() > 0);
        }
    }
}