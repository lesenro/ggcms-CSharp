using GgcmsCSharp.Models;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;

namespace GgcmsCSharp.ApiCtrls
{
    public class GgcmsMembersController : ApiController
    {
        private dbTools<GgcmsMember> dbtool;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            this.dbtool = new dbTools<GgcmsMember>(Request);
        }
        // GET: api/GgcmsCategories
        [HttpGet]
        public ResultData GetList()
        {
            return dbtool.GetList();
        }

        // GET: api/GgcmsCategories/5
        public ResultData GetInfo(int id)
        {
            return dbtool.GetById(id);
        }

        // PUT: api/GgcmsCategories/5
        public ResultData Edit(int id, GgcmsMember GgcmsMember)
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

            return dbtool.Edit(id, GgcmsMember);
        }

        // POST: api/GgcmsCategories
        public ResultData Add(GgcmsMember GgcmsMember)
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
            return dbtool.Add(GgcmsMember);
        }

        // DELETE: api/GgcmsCategories/5
        public ResultData Delete(int id)
        {
            return dbtool.Delete(id);
        }
        [HttpGet]
        public ResultData MultDelete()
        {
            return dbtool.MultDelete();
        }
        public ResultData ModifyPassword(dynamic passData)
        {
            string oldPassword = passData.oldPassword.ToString();
            string newPassword = passData.newPassword.ToString();
            string rePassword = passData.rePassword.ToString();
            var session = HttpContext.Current.Session;
            ResultData result = new ResultData
            {
                Code = 1,
                Msg = ""
            };
            if (session != null)
            {
                if (session["ggcms_loginUser"] != null)
                {
                    GgcmsMember m = session["ggcms_loginUser"] as GgcmsMember;
                    GgcmsMember info = dbtool.db.GgcmsMembers.Find(m.Id);
                    if (info.PassWord != oldPassword)
                    {
                        result.Msg = "原密码不正确";
                    }
                    else if (newPassword != rePassword)
                    {
                        result.Msg = "新密码和确认密码不同";
                    }
                    else
                    {
                        info.PassWord = newPassword;
                        var ent = dbtool.db.Entry(info);
                        ent.Property("PassWord").IsModified = true;
                        dbtool.db.SaveChanges();
                        result.Code = 0;
                        session.RemoveAll();
                    }
                }
            }

            return result;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbtool.Dispose(true);
            }
            base.Dispose(disposing);
        }

        public ResultData Exists(int id)
        {
            return dbtool.Exists(id);
        }
    }
}