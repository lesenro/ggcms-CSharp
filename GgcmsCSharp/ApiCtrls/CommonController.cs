using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Web.Http;
using System.Reflection;
using System.Collections;

namespace GgcmsCSharp.ApiCtrls
{
    public class CommonController : ApiController
    {
        [HttpPost]
        public dynamic fileUpload()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            string staticDir = ConfigurationManager.AppSettings["StaticDir"].ToString();
            string uploadDir = ConfigurationManager.AppSettings["UploadDir"].ToString();

            string root = "/" + staticDir + "/" + uploadDir + "/temp";
            string rootpath = HttpContext.Current.Server.MapPath("~" + root);
            if (!Directory.Exists(rootpath))
            {
                Directory.CreateDirectory(rootpath);
            }

            ResultData result = new ResultData
            {
                Code = 0,
                Msg = "",
            };
            string link = "";
            var httpRequest = HttpContext.Current.Request;
            string serverurl = httpRequest.Form["serverUrl"];
            if (httpRequest.Files.Count > 0)
            {

                var docfiles = new List<dynamic>();
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath("~" + root + "/" + postedFile.FileName);
                    string ext = Path.GetExtension(filePath);
                    string fn = postedFile.FileName;

                    while (File.Exists(filePath))
                    {
                        fn = Path.GetFileName(Path.GetTempFileName()) + ext;
                        filePath = HttpContext.Current.Server.MapPath("~" + root + "/" + fn);
                    }
                    postedFile.SaveAs(filePath);

                    docfiles.Add(new
                    {
                        url = root + "/" + fn,
                        fname = postedFile.FileName
                    });
                    link = root + "/" + fn;
                }
                result.Data = docfiles;
                result.Msg = "ok";
            }
            else
            {
                result.Code = 1;
                result.Msg = "no file";
            }
            link = serverurl + link;
            link = link.Replace("//", "/").Replace(":/", "://");
            return new
            {
                Code = result.Code,
                Msg = result.Code,
                Data = result.Data,
                link = link,
            };
        }
        [HttpGet]
        public ResultData ClearCache()
        {
            try
            {
                List<string> pages = CacheHelper.GetPages();
                foreach (string key in pages)
                {
                    HttpResponse.RemoveOutputCacheItem(key);
                }
                //必须放后面
                CacheHelper.RemoveAllCache();

                return new ResultData
                {
                    Code = 0,
                    Msg = "ok"
                };
            }
            catch (Exception ex)
            {
                return new ResultData
                {
                    Code = 1,
                    Msg = ex.Message,

                };
            }
        }
        [HttpGet]
        public ResultData AppRestart()
        {
            try
            {
                HttpRuntime.UnloadAppDomain();
                return new ResultData
                {
                    Code = 0,
                    Msg = "ok"
                };
            }
            catch (Exception ex)
            {
                return new ResultData
                {
                    Code = 1,
                    Msg = ex.Message,

                };
            }
        }
    }
}
