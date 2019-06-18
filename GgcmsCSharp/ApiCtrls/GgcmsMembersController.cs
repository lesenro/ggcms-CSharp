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
    public class GgcmsMembersController : ApiBaseController
    {

        // GET: api/GgcmsCategories
        [HttpGet]
        public IHttpActionResult GetList(string query)
        {
            string json = HttpUtility.UrlDecode(query);
            SearchParams sParams = Tools.JsonDeserialize<SearchParams>(json);

            return Ok(GetRecords<GgcmsMembers>(sParams));
        }

        // GET: api/GgcmsCategories/5
        public IHttpActionResult GetInfo(int id)
        {
            return Ok(Dbctx.GgcmsMembers.Find(id));
        }

        // PUT: api/GgcmsCategories/5
        public IHttpActionResult Edit(GgcmsMembers info)
        {
            if (Dbctx.GgcmsMembers.Where(x => x.Id == info.Id).Count() == 0)
            {
                return BadRequest("信息不存在");
            }
            //Dbctx.GgcmsMembers.Attach(info);
            //Dbctx.Entry(info).Property("goods_name").IsModified = true;
            var ent = Dbctx.Entry(info);
            ent.State = EntityState.Modified;
            Dbctx.SaveChanges();
            ClearCache();
            return Ok(info);

        }

        // POST: api/GgcmsCategories
        public IHttpActionResult Add(GgcmsMembers info)
        {
            var result = Dbctx.GgcmsMembers.Add(info);
            Dbctx.SaveChanges();
            ClearCache();
            return Ok(result);

        }

        // DELETE: api/GgcmsCategories/5
        public IHttpActionResult Delete(int id)
        {
            GgcmsMembers oldinfo = Dbctx.GgcmsMembers.Find(id);
            if (oldinfo == null)
            {
                return BadRequest("信息不存在");
            }

            //List<int> idlist = GetDeleteIds(oldinfo.ticket_key);

            //var query = Dbctx.ticket_information.Where(x => idlist.Contains(x.id));
            Dbctx.GgcmsMembers.Remove(oldinfo);
            Dbctx.SaveChanges();
            ClearCache();
            return Ok(oldinfo);
        }
        [HttpPost]
        public IHttpActionResult MultDelete(int[] ids)
        {
            var query = Dbctx.GgcmsMembers.Where(x => ids.Contains(x.Id));

            Dbctx.GgcmsMembers.RemoveRange(query);
            int c = Dbctx.SaveChanges();
            ClearCache();
            return Ok(c);
        }
        public IHttpActionResult ModifyPassword(dynamic passData)
        {
            string oldPassword = passData.oldPassword.ToString();
            string newPassword = passData.newPassword.ToString();
            string rePassword = passData.rePassword.ToString();
            string sessionKey = SystemEnums.login_user.ToString();

            if (Session[sessionKey] != null)
            {
                GgcmsMembers m = Session[sessionKey] as GgcmsMembers;
                GgcmsMembers info = Dbctx.GgcmsMembers.Find(m.Id);
                if (info.PassWord != oldPassword)
                {
                    return BadRequest("原密码不正确");
                }
                else if (newPassword != rePassword)
                {
                    return BadRequest("新密码和确认密码不同");
                }
                else
                {
                    info.PassWord = newPassword;
                    var ent = Dbctx.Entry(info);
                    ent.Property("PassWord").IsModified = true;
                    Dbctx.SaveChanges();
                    Session.RemoveAll();
                }
                return Ok(info);
            }
            return BadRequest("请先登录");

        }

        public IHttpActionResult Exists(int id)
        {
            return Ok(Dbctx.GgcmsMembers.Where(x => x.Id == id).Count() > 0);

        }
    }
}