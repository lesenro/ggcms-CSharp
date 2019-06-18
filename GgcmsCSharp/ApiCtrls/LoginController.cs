using GgcmsCSharp.Models;
using GgcmsCSharp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace GgcmsCSharp.ApiCtrls
{
    //  [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class LoginController : ApiBaseController
    {
        private GgcmsDB ggcmsDb = new GgcmsDB();


        [HttpPost]
        //[GgcmsAuthorizeAttribute("vip")]
        public IHttpActionResult PostLogin(dynamic linfo)
        {

            string username = linfo.username;
            string password = linfo.password;
            string captcha = linfo.captcha;
            string sessionKey = SystemEnums.captcha.ToString();

            if (Session[sessionKey] != null)
            {
                string sn_captcha = Session[sessionKey].ToString();
                if (sn_captcha.ToLower() != captcha.ToLower())
                {
                    return BadRequest("验证码不正确");
                }
            }
            else
            {
                return BadRequest("验证码不正确");
            }
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return BadRequest("用户名和密码不能为空");
            }
            var member = from c in ggcmsDb.GgcmsMembers
                         where c.UserName == username
                         select c;
            GgcmsMembers m = member.FirstOrDefault<GgcmsMembers>();
            if (m == null)
            {
                return BadRequest("用户名或密码错误，请检查!");
            }
            else
            {
                string md5pass = Tools.getMd5Hash(m.PassWord + captcha);
                if (md5pass != password)
                {
                    return BadRequest("用户名或密码错误，请检查...");
                }
            }
            m.PassWord = "";
            Session.Add(SystemEnums.login_user.ToString(), m);
            return Ok(m);
        }
        [HttpGet]
        public IHttpActionResult GetLoginInfo()
        {
            var user = GetLoginUser();
            if (user != null)
            {
                return Ok(user);
            }
            return BadRequest("无登录用户");
        }
        [HttpGet]
        public IHttpActionResult GetLogout()
        {

            Session.RemoveAll();

            return Ok("退出成功");
        }

    }
}
