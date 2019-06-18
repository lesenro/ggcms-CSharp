using GgcmsCSharp.Models;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System;
using GgcmsCSharp.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GgcmsCSharp.Utils;
using System.Data.Entity;

namespace GgcmsCSharp.ApiCtrls
{
    public class GgcmsAdvertsController : ApiBaseController
    {
        

        // GET: api/GgcmsCategories
        [HttpGet]
        public IHttpActionResult GetList(string query)
        {
            string json = HttpUtility.UrlDecode(query);
            SearchParams sParams = Tools.JsonDeserialize<SearchParams>(json);

            return Ok(GetRecords<GgcmsAdverts>(sParams));
        }

        // GET: api/GgcmsCategories/5
        public IHttpActionResult GetInfo(int id)
        {
            return Ok(Dbctx.GgcmsAdverts.Find(id));
        }

        // PUT: api/GgcmsCategories/5
        public IHttpActionResult Edit(GgcmsAdverts info)
        {
            if (Dbctx.GgcmsAdverts.Where(x => x.Id == info.Id).Count() == 0)
            {
                return BadRequest("信息不存在");
            }

            UpFileClass.FileSave(info, info.files.FindAll(x => x.fileType != 3));
            //Dbctx.GgcmsAdverts.Attach(info);
            //Dbctx.Entry(info).Property("goods_name").IsModified = true;
            var ent = Dbctx.Entry(info);
            ent.State = EntityState.Modified;

            Dbctx.SaveChanges();
            ClearCache();
            return Ok(info);
        }

        // POST: api/GgcmsCategories
        public IHttpActionResult Add(GgcmsAdverts info)
        {
            UpFileClass.FileSave(info, info.files.FindAll(x => x.fileType != 3));
            var result = Dbctx.GgcmsAdverts.Add(info);
            Dbctx.SaveChanges();
            ClearCache();
            return Ok(result);
        }

        // DELETE: api/GgcmsCategories/5
        public IHttpActionResult Delete(int id)
        {
            GgcmsAdverts oldinfo = Dbctx.GgcmsAdverts.Find(id);
            if (oldinfo == null)
            {
                return BadRequest("信息不存在");
            }

            //List<int> idlist = GetDeleteIds(oldinfo.ticket_key);

            //var query = Dbctx.ticket_information.Where(x => idlist.Contains(x.id));
            Dbctx.GgcmsAdverts.Remove(oldinfo);
            Dbctx.SaveChanges();
            ClearCache();
            return Ok(oldinfo);
        }
        [HttpPost]
        public IHttpActionResult MultDelete(int[] ids)
        {
            var query = Dbctx.GgcmsAdverts.Where(x => ids.Contains(x.Id));

            Dbctx.GgcmsAdverts.RemoveRange(query);
            int c = Dbctx.SaveChanges();
            ClearCache();
            return Ok(c);

        }
        

        public IHttpActionResult Exists(int id)
        {
            return Ok(Dbctx.GgcmsAdverts.Where(x => x.Id == id).Count() > 0);
        }
    }
}