using GgcmsCSharp.Models;
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
        public ResultData GetList()
        {
            var reqParams = InitRequestParams<GgcmsMember>();
            var result = dbHelper.GetList<GgcmsMember>(reqParams);
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
            var result = dbHelper.GetById<GgcmsMember>(id);
            return new ResultData
            {
                Code = 0,
                Data = result,
                Msg = ""
            };
        }

        // PUT: api/GgcmsCategories/5
        public ResultData Edit(int id, GgcmsMember member)
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

            return new ResultData
            {
                Code = 0,
                Data = dbHelper.Edit(member.Id, member),
                Msg = ""
            };
        }

        // POST: api/GgcmsCategories
        public ResultData Add(GgcmsMember member)
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
            return new ResultData
            {
                Code = 0,
                Msg = "",
                Data = dbHelper.Add(member)
            };
        }

        // DELETE: api/GgcmsCategories/5
        public ResultData Delete(int id)
        {
            return new ResultData
            {
                Code = 0,
                Msg = "",
                Data = dbHelper.Delete<GgcmsMember>(id)
            };
        }
        [HttpGet]
        public ResultData MultDelete()
        {
            var reqParams = InitRequestParams<GgcmsMember>();
            return new ResultData
            {
                Code = 0,
                Msg = "",
                Data = dbHelper.MultDelete<GgcmsMember>(reqParams)
            };
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
                    GgcmsMember info = dbHelper.dbCxt.GgcmsMembers.Find(m.Id);
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
                        var ent = dbHelper.dbCxt.Entry(info);
                        ent.Property("PassWord").IsModified = true;
                        dbHelper.dbCxt.SaveChanges();
                        result.Code = 0;
                        session.RemoveAll();
                    }
                }
            }

            return result;
        }

        public ResultData Exists(int id)
        {
            return new ResultData
            {
                Code = 0,
                Msg = "",
                Data = dbHelper.Exists<GgcmsMember>(id)
            };
        }
    }
}