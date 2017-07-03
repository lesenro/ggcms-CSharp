using GgcmsCSharp.Models;
using GgcmsCSharp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace GgcmsCSharp.ApiCtrls
{
    //  [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class LoginController : ApiController
    {
        private GgcmsDB ggcmsDb = new GgcmsDB();

        public class loginInfo
        {
            public string username { get; set; }
            public string password { get; set; }
            public string captcha { get; set; }

        }
        [HttpPost]
        //[GgcmsAuthorizeAttribute("vip")]
        public ResultData PostLogin([FromBody] loginInfo linfo)
        {
            ResultData result = new ResultData
            {
                Code = 0,
                Msg = ""
            };
            /*
            GgcmsDB db = new GgcmsDB();
            int count = db.GgcmsMembers.Count();
            if (count == 0)
            {
                db.GgcmsMembers.Add(new GgcmsMember
                {
                    UserName = "admin",
                    PassWord = Tools.getMd5Hash("123456"),
                    JoinTime = DateTime.Now,
                });
                try {
                    db.SaveChanges();
                }
                catch (Exception ex) {

                }
            }*/
            var session = HttpContext.Current.Session;
            if (session != null)
            {
                if (session["ggcms_code"] != null)
                {
                    string sn_captcha = session["ggcms_code"].ToString();
                    if (sn_captcha.ToLower() != linfo.captcha.ToLower())
                    {
                        result.Code = 101;
                        result.Msg = "验证码不正确";
                        return result;
                    }
                }
                else
                {
                    result.Code = 101;
                    return result;
                }
            }
            if (string.IsNullOrEmpty(linfo.username) || string.IsNullOrEmpty(linfo.password))
            {
                result.Code = 102;
                result.Msg = "用户名和密码不能为空";
                return result;
            }
            var member = from c in ggcmsDb.GgcmsMembers
                         where c.UserName == linfo.username
                         select c;
            GgcmsMember m = member.FirstOrDefault<GgcmsMember>();
            if (m == null)
            {
                result.Code = 103;
                result.Msg = "用户名或密码错误，请检查!";
                return result;
            }
            else
            {
                string md5pass = Tools.getMd5Hash(m.PassWord + linfo.captcha);
                if (md5pass != linfo.password)
                {
                    result.Code = 104;
                    result.Msg = "用户名或密码错误，请检查...";
                    return result;
                }
            }
            m.PassWord = "";
            session.Add("ggcms_loginUser", m);
            result.Data = m;
            return result;
        }
        [HttpGet]
        public ResultData GetLoginUser()
        {
            ResultData result = new ResultData
            {
                Code = 1,
                Msg = "无登录用户"
            };
            var session = HttpContext.Current.Session;
            if (session != null)
            {
                if (session["ggcms_loginUser"] != null)
                {
                    GgcmsMember m = session["ggcms_loginUser"] as GgcmsMember;
                    result = new ResultData
                    {
                        Code = 0,
                        Data = m
                    };
                }
            }
            return result;
        }
        [HttpGet]
        public ResultData GetLogout()
        {
            ResultData result = new ResultData
            {
                Code = 0,
                Msg = ""
            };

            var session = HttpContext.Current.Session;
            if (session != null)
            {

                session.RemoveAll();
            }

            return result;
        }

    }
}
