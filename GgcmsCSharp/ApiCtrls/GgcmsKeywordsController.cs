using GgcmsCSharp.Models;
using GgcmsCSharp.Utils;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;

namespace GgcmsCSharp.ApiCtrls
{
    public class GgcmsKeywordsController : ApiBaseController
    {


        // GET: api/GgcmsCategories
        [HttpGet]
        public IHttpActionResult GetList(string query)
        {
            string json = HttpUtility.UrlDecode(query);
            SearchParams sParams = Tools.JsonDeserialize<SearchParams>(json);

            return Ok(GetRecords<GgcmsKeywords>(sParams));
        }

        // GET: api/GgcmsCategories/5
        public IHttpActionResult GetInfo(int id)
        {
            return Ok(Dbctx.GgcmsKeywords.Find(id));

        }

        // PUT: api/GgcmsCategories/5
        public IHttpActionResult Edit(int id, GgcmsKeywords info)
        {

            if (Dbctx.GgcmsKeywords.Where(x => x.Id == info.Id).Count() == 0)
            {
                return BadRequest("信息不存在");
            }
            //Dbctx.GgcmsKeywords.Attach(info);
            //Dbctx.Entry(info).Property("goods_name").IsModified = true;
            var ent = Dbctx.Entry(info);
            ent.State = EntityState.Modified;
            Dbctx.SaveChanges();
            ClearCache();
            return Ok(info);
        }

        // POST: api/GgcmsCategories
        public IHttpActionResult Add(GgcmsKeywords info)
        {
            var result = Dbctx.GgcmsKeywords.Add(info);
            Dbctx.SaveChanges();
            ClearCache();
            return Ok(result);
        }

        // DELETE: api/GgcmsCategories/5
        public IHttpActionResult Delete(int id)
        {
            GgcmsKeywords oldinfo = Dbctx.GgcmsKeywords.Find(id);
            if (oldinfo == null)
            {
                return BadRequest("信息不存在");
            }

            //List<int> idlist = GetDeleteIds(oldinfo.ticket_key);

            //var query = Dbctx.ticket_information.Where(x => idlist.Contains(x.id));
            Dbctx.GgcmsKeywords.Remove(oldinfo);
            Dbctx.SaveChanges();
            ClearCache();
            return Ok(oldinfo);
        }
        [HttpPost]
        public IHttpActionResult MultDelete(int[] ids)
        {
            var query = Dbctx.GgcmsKeywords.Where(x => ids.Contains(x.Id));

            Dbctx.GgcmsKeywords.RemoveRange(query);
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