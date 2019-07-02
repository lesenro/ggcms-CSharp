using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GgcmsCSharp.ApiCtrls;
using System.IO;
using System.Configuration;
using GgcmsCSharp.Models;

namespace GgcmsCSharp.Utils
{
    public class LocalSave : IThirdPartyStorage
    {
        public string serverUrl { get; set; }

        private string root { get; set; }
        private string rootpath { get; set; }
        private string UploadPrefix { get; set; }
        public LocalSave()
        {
            serverUrl = "";
            
            string staticDir = ConfigurationManager.AppSettings["StaticDir"].ToString();
            string uploadDir = ConfigurationManager.AppSettings["UploadDir"].ToString();
            UploadPrefix = ConfigurationManager.AppSettings["ServerBaseUrl"].ToString();
            serverUrl = UploadPrefix;
            DateTime dtime = DateTime.Now;
            root = "/" + staticDir + "/" + uploadDir + "/" + dtime.ToString("yyyyMM");
            rootpath = HttpContext.Current.Server.MapPath("~" + root);
            if (!Directory.Exists(rootpath))
            {
                Directory.CreateDirectory(rootpath);
            }
        }
        public ResultInfo Delete(string key)
        {
            string filePath= HttpContext.Current.Server.MapPath("~" + key);
            File.Delete(filePath);
            return new ResultInfo
            {
                Code = 0,
                Msg = "",
            };
        }

        public ResultInfo UploadFile(string file)
        {
            string tempFile = HttpContext.Current.Server.MapPath("~" + file);
            string ext = Path.GetExtension(tempFile);
            string fn = Path.GetFileName(tempFile);
            string filePath = HttpContext.Current.Server.MapPath("~" + root + "/" + fn);
            string url = UploadPrefix + root + "/" + fn;
            while (File.Exists(filePath))
            {
                fn = Path.GetFileName(Tools.getRandString(8)) + ext;
                filePath = HttpContext.Current.Server.MapPath("~" + root + "/" + fn);
                url = UploadPrefix+root + "/" + fn;
            }
            File.Move(tempFile, filePath);
            return new ResultInfo
            {
                Code = 0,
                Msg = "",
                Data = url,
            };
        }
    }
}