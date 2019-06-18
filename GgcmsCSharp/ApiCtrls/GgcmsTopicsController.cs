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
    public class GgcmsTopicsController : ApiBaseController
    {

        // GET: api/GgcmsCategories
        [HttpGet]
        public IHttpActionResult GetList(string query)
        {
            string json = HttpUtility.UrlDecode(query);
            SearchParams sParams = Tools.JsonDeserialize<SearchParams>(json);

            return Ok(GetRecords<GgcmsTopics>(sParams));
        }

        // GET: api/GgcmsCategories/5
        public IHttpActionResult GetInfo(int id)
        {
            return Ok(Dbctx.GgcmsTopics.Find(id));
        }

        // PUT: api/GgcmsCategories/5
        public IHttpActionResult Edit(GgcmsTopics info)
        {
            if (Dbctx.GgcmsTopics.Where(x => x.Id == info.Id).Count() == 0)
            {
                return BadRequest("信息不存在");
            }
            //Dbctx.GgcmsTopics.Attach(info);
            //Dbctx.Entry(info).Property("goods_name").IsModified = true;
            var ent = Dbctx.Entry(info);
            ent.State = EntityState.Modified;
            Dbctx.SaveChanges();
            ClearCache();
            return Ok(info);
        }

        // POST: api/GgcmsCategories
        public IHttpActionResult Add(GgcmsTopics info)
        {
            var result = Dbctx.GgcmsTopics.Add(info);
            Dbctx.SaveChanges();
            ClearCache();
            return Ok(result);
        }

        // DELETE: api/GgcmsCategories/5
        public IHttpActionResult Delete(int id)
        {
            GgcmsTopics oldinfo = Dbctx.GgcmsTopics.Find(id);
            if (oldinfo == null)
            {
                return BadRequest("信息不存在");
            }

            //List<int> idlist = GetDeleteIds(oldinfo.ticket_key);

            //var query = Dbctx.ticket_information.Where(x => idlist.Contains(x.id));
            Dbctx.GgcmsTopics.Remove(oldinfo);
            Dbctx.SaveChanges();
            ClearCache();
            return Ok(oldinfo);
        }
        [HttpPost]
        public IHttpActionResult MultDelete(int[] ids)
        {
            var query = Dbctx.GgcmsTopics.Where(x => ids.Contains(x.Id));

            Dbctx.GgcmsTopics.RemoveRange(query);
            int c = Dbctx.SaveChanges();
            ClearCache();
            return Ok(c);
        }


        public IHttpActionResult Exists(int id)
        {
            return Ok(Dbctx.GgcmsTopics.Where(x => x.Id == id).Count() > 0);
        }
    }
}