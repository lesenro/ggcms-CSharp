using GgcmsCSharp.Models;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Linq;
using System.Configuration;
using System.Web;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System;

namespace GgcmsCSharp.ApiCtrls
{
    public class GgcmsStylesController : ApiController
    {
        private dbTools<GgcmsStyle> dbtool;
        private FileManager fman;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            this.dbtool = new dbTools<GgcmsStyle>(Request);
            this.fman = new FileManager();
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
        public ResultData Edit(GgcmsStyle styleInfo)
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
            GgcmsDB db = new GgcmsDB();
            var qlist = from r in db.GgcmsStyles
                        where r.Id == styleInfo.Id
                        select r;
            GgcmsStyle oldinfo = qlist.First();
            styleInfo.Folder = oldinfo.Folder;
            return dbtool.Edit(styleInfo.Id, styleInfo);
        }

        // POST: api/GgcmsCategories
        public ResultData Add(GgcmsStyle styleInfo)
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
            string staticDir = ConfigurationManager.AppSettings["StaticDir"].ToString();
            string styleDir = ConfigurationManager.AppSettings["StyleDir"].ToString();
            string root = "/" + staticDir + "/" + styleDir + "/" + styleInfo.Folder;
            string templateDir = ConfigurationManager.AppSettings["TemplateDir"].ToString();
            templateDir = "/Views/" + templateDir + "/" + styleInfo.Folder;

            string rootpath = HttpContext.Current.Server.MapPath("~" + root);
            string templatePath = HttpContext.Current.Server.MapPath("~" + templateDir);
            if (Directory.Exists(rootpath) || Directory.Exists(templatePath))
            {
                return new ResultData
                {
                    Code = 1,
                    Msg = "文件夹已经存在",
                };
            }
            else
            {
                Directory.CreateDirectory(rootpath);
                Directory.CreateDirectory(templatePath);
            }
            return dbtool.Add(styleInfo);
        }

        // DELETE: api/GgcmsCategories/5
        public ResultData Delete(int id)
        {
            return dbtool.Delete(id);
        }
        [HttpGet]
        public ResultData MultDelete()
        {
            string staticDir = ConfigurationManager.AppSettings["StaticDir"].ToString();
            string styleDir = ConfigurationManager.AppSettings["StyleDir"].ToString();
            styleDir = "/" + staticDir + "/" + styleDir + "/";
            string templateDir = ConfigurationManager.AppSettings["TemplateDir"].ToString();
            templateDir = "/Views/" + templateDir + "/";
            try
            {
                IQueryable list = dbtool.GetInfoList();
                foreach (var item in list)
                {
                    GgcmsStyle styleInfo = item as GgcmsStyle;
                    string stylePath = HttpContext.Current.Server.MapPath("~" + styleDir + styleInfo.Folder);
                    string templatePath = HttpContext.Current.Server.MapPath("~" + templateDir + styleInfo.Folder);
                    Directory.Delete(stylePath, true);
                    Directory.Delete(templatePath, true);
                }
            }
            catch (Exception ex)
            {
                return new ResultData
                {
                    Code = 1,
                    Msg = ex.Message,
                    Data = ex
                };
            }
            return dbtool.MultDelete();
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
        [HttpGet]
        public ResultData GetStaticFile(int id, string path)
        {
            path = HttpUtility.UrlDecode(path);
            ResultData rdata = dbtool.GetById(id);
            GgcmsStyle oldinfo = rdata.Data as GgcmsStyle;
            string staticDir = ConfigurationManager.AppSettings["StaticDir"].ToString();
            string styleDir = ConfigurationManager.AppSettings["StyleDir"].ToString();
            string root = staticDir + "/" + styleDir + "/" + oldinfo.Folder + "/" + path;
            return fman.GetList(root);
        }
        [HttpGet]
        public ResultData GetStaticFileInfo(int id, string path)
        {
            path = HttpUtility.UrlDecode(path);
            ResultData rdata = dbtool.GetById(id);
            GgcmsStyle sinfo = rdata.Data as GgcmsStyle;
            string staticDir = ConfigurationManager.AppSettings["StaticDir"].ToString();
            string styleDir = ConfigurationManager.AppSettings["StyleDir"].ToString();
            string root = staticDir + "/" + styleDir + "/" + sinfo.Folder + "/" + path;
            return fman.GetInfo(root);

        }
        public ResultData StaticFileSave(dynamic dataInfo)
        {
            int id = (int)dataInfo.id;
            string path = dataInfo.path.ToString();
            path = HttpUtility.UrlDecode(path);
            string content = dataInfo.content.ToString();
            ResultData rdata = dbtool.GetById(id);
            GgcmsStyle sinfo = rdata.Data as GgcmsStyle;
            string staticDir = ConfigurationManager.AppSettings["StaticDir"].ToString();
            string styleDir = ConfigurationManager.AppSettings["StyleDir"].ToString();
            string fullname = staticDir + "/" + styleDir + "/" + sinfo.Folder + "/" + path;
            return fman.Save(fullname, content);

        }

        public ResultData StaticFileDelete(dynamic dataInfo)
        {
            int id = (int)dataInfo.id;
            string path = dataInfo.path.ToString();
            path = HttpUtility.UrlDecode(path);
            dynamic files = dataInfo.files;
            ResultData rdata = dbtool.GetById(id);
            GgcmsStyle sinfo = rdata.Data as GgcmsStyle;
            string staticDir = ConfigurationManager.AppSettings["StaticDir"].ToString();
            string styleDir = ConfigurationManager.AppSettings["StyleDir"].ToString();
            string fullname = staticDir + "/" + styleDir + "/" + sinfo.Folder + "/" + path;
            return fman.Delete(fullname, files);

        }
        //新建文件夹
        public ResultData StaticFileNewDir(dynamic dataInfo)
        {
            int id = (int)dataInfo.id;
            string path = dataInfo.path.ToString();
            string newName = dataInfo.newName.ToString();
            path = HttpUtility.UrlDecode(path);
            ResultData rdata = dbtool.GetById(id);
            GgcmsStyle sinfo = rdata.Data as GgcmsStyle;
            string staticDir = ConfigurationManager.AppSettings["StaticDir"].ToString();
            string styleDir = ConfigurationManager.AppSettings["StyleDir"].ToString();
            string fullname = staticDir + "/" + styleDir + "/" + sinfo.Folder + "/" + path + "/" + newName;
            return fman.NewDir(fullname);

        }
        //重命名
        public ResultData StaticFileReName(dynamic dataInfo)
        {
            int id = (int)dataInfo.id;
            string path = dataInfo.path.ToString();
            string newName = dataInfo.newName.ToString();
            string oldName = dataInfo.oldName.ToString();
            ResultData rdata = dbtool.GetById(id);
            GgcmsStyle sinfo = rdata.Data as GgcmsStyle;
            string staticDir = ConfigurationManager.AppSettings["StaticDir"].ToString();
            string styleDir = ConfigurationManager.AppSettings["StyleDir"].ToString();
            string nname = staticDir + "/" + styleDir + "/" + sinfo.Folder + "/" + path + "/" + newName;
            string oname = staticDir + "/" + styleDir + "/" + sinfo.Folder + "/" + path + "/" + oldName;
            nname = HttpUtility.UrlDecode(nname);
            oname = HttpUtility.UrlDecode(oname);
            return fman.StaticFileReName(oname, nname);

        }
        //上传
        public ResultData StaticFileUpload()
        {
            var httpRequest = HttpContext.Current.Request;
            int id = int.Parse(httpRequest.Form["id"]);
            string path = httpRequest.Form["path"];
            path = HttpUtility.UrlDecode(path);
            ResultData rdata = dbtool.GetById(id);
            GgcmsStyle sinfo = rdata.Data as GgcmsStyle;
            string staticDir = ConfigurationManager.AppSettings["StaticDir"].ToString();
            string styleDir = ConfigurationManager.AppSettings["StyleDir"].ToString();
            string fullname = staticDir + "/" + styleDir + "/" + sinfo.Folder + "/" + path;

            return FileUpload(httpRequest, fullname);

        }
        private ResultData FileUpload(HttpRequest httpRequest, string path)
        {
            ResultData result = new ResultData
            {
                Code = 0,
                Msg = "",
            };
            try
            {
                if (httpRequest.Files.Count > 0)
                {
                    List<string> fs = new List<string>();
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        var filePath = HttpContext.Current.Server.MapPath("~/" + path + "/" + postedFile.FileName);
                        string ext = Path.GetExtension(filePath);
                        string fn = postedFile.FileName;

                        while (File.Exists(filePath))
                        {
                            fn = Path.GetFileName(Path.GetTempFileName()) + ext;
                            filePath = HttpContext.Current.Server.MapPath("~/" + path + "/" + fn);
                        }
                        postedFile.SaveAs(filePath);
                        fs.Add(filePath);
                    }
                    result.Msg = "ok";
                    result.Data = fs;
                }
            }
            catch (Exception ex)
            {
                result.Code = 1;
                result.Msg = ex.Message;
                result.Data = ex;
            }
            return result;

        }

        [HttpGet]
        public ResultData GetTemplateList(int id)
        {
            ResultData rdata = dbtool.GetById(id);
            GgcmsStyle styleInfo = rdata.Data as GgcmsStyle;
            string templateDir = ConfigurationManager.AppSettings["TemplateDir"].ToString();
            templateDir = "Views/" + templateDir + "/" + styleInfo.Folder;
            return fman.GetList(templateDir);
        }
        [HttpGet]
        public ResultData GetTemplateInfo(int id, string filename)
        {
            filename = HttpUtility.UrlDecode(filename);
            ResultData rdata = dbtool.GetById(id);
            GgcmsStyle styleInfo = rdata.Data as GgcmsStyle;
            string templateDir = ConfigurationManager.AppSettings["TemplateDir"].ToString();
            templateDir = "Views/" + templateDir + "/" + styleInfo.Folder + "/" + filename;

            return fman.GetInfo(templateDir);
        }
        public ResultData TemplateFileSave(dynamic dataInfo)
        {
            int id = (int)dataInfo.id;
            string filename = dataInfo.filename.ToString();
            string content = dataInfo.content.ToString();
            ResultData rdata = dbtool.GetById(id);
            GgcmsStyle sinfo = rdata.Data as GgcmsStyle;
            string templateDir = ConfigurationManager.AppSettings["TemplateDir"].ToString();
            string fullname = "Views/" + templateDir + "/" + sinfo.Folder + "/" + filename;
            return fman.Save(fullname, content);
        }
        public ResultData TemplateFileDelete(dynamic dataInfo)
        {
            int id = (int)dataInfo.id;
            dynamic files = dataInfo.files;
            ResultData rdata = dbtool.GetById(id);
            GgcmsStyle sinfo = rdata.Data as GgcmsStyle;
            string templateDir = ConfigurationManager.AppSettings["TemplateDir"].ToString();
            string fullname = "Views/" + templateDir + "/" + sinfo.Folder;
            return fman.Delete(fullname, files);

        }
        public ResultData TemplateFileUpload()
        {
            var httpRequest = HttpContext.Current.Request;
            int id = int.Parse(httpRequest.Form["id"]);
            ResultData rdata = dbtool.GetById(id);
            GgcmsStyle sinfo = rdata.Data as GgcmsStyle;
            string templateDir = ConfigurationManager.AppSettings["TemplateDir"].ToString();
            string fullname = "Views/" + templateDir + "/" + sinfo.Folder;

            return FileUpload(httpRequest, fullname);

        }
        public ResultData TemplateFileReName(dynamic dataInfo)
        {
            int id = (int)dataInfo.id;
            string newName = dataInfo.newName.ToString();
            string oldName = dataInfo.oldName.ToString();
            ResultData rdata = dbtool.GetById(id);
            GgcmsStyle sinfo = rdata.Data as GgcmsStyle;
            string templateDir = ConfigurationManager.AppSettings["TemplateDir"].ToString();

            string nname = "Views/" + templateDir + "/" + sinfo.Folder + "/" + newName;
            string oname = "Views/" + templateDir + "/" + sinfo.Folder + "/" + oldName;
            nname = HttpUtility.UrlDecode(nname);
            oname = HttpUtility.UrlDecode(oname);
            return fman.StaticFileReName(oname, nname);

        }
        [HttpGet]
        public void StyleExport(int id)
        {
            ResultData rdata = dbtool.GetById(id);
            GgcmsStyle sinfo = rdata.Data as GgcmsStyle;
            string templateDir = ConfigurationManager.AppSettings["TemplateDir"].ToString();
            string staticDir = ConfigurationManager.AppSettings["StaticDir"].ToString();
            string styleDir = ConfigurationManager.AppSettings["StyleDir"].ToString();
            string uploadDir = ConfigurationManager.AppSettings["UploadDir"].ToString();
            //模板目录
            string viewPath = "Views/" + templateDir + "/" + sinfo.Folder;
            string viewDir = HttpContext.Current.Server.MapPath("~/" + viewPath);
            //风格目录
            string stylePath = staticDir + "/" + styleDir + "/" + sinfo.Folder;
            string sDir = HttpContext.Current.Server.MapPath("~/" + stylePath);
            //模板要复制的临时目录
            string tmplPath = HttpContext.Current.Server.MapPath("~/" + stylePath + "/template");

            if (!Directory.Exists(tmplPath))
            {
                Directory.CreateDirectory(tmplPath);
            }
            else
            {
                Directory.Delete(tmplPath, true);
                Directory.CreateDirectory(tmplPath);
            }

            string[] tmplFiles = Directory.GetFiles(viewDir);
            foreach (string f in tmplFiles)
            {
                string fn = Path.GetFileName(f);
                File.Copy(f, tmplPath + "/" + fn);
            }
            //生成的zip文件
            string zipFilePath = staticDir + "/" + uploadDir + "/temp/" + Path.GetFileName(Path.GetTempFileName()) + ".zip";
            string zipFile = HttpContext.Current.Server.MapPath("~/" + zipFilePath);
            while (File.Exists(zipFile))
            {
                zipFilePath = staticDir + "/" + uploadDir + "/temp/" + Path.GetFileName(Path.GetTempFileName()) + ".zip";
                zipFile = HttpContext.Current.Server.MapPath("~/" + zipFilePath);
            }
            string extDir = Path.GetDirectoryName(zipFile);
            if (!Directory.Exists(extDir))
            {
                Directory.CreateDirectory(extDir);
            }
            ZipFile.CreateFromDirectory(sDir, zipFile);
            Directory.Delete(tmplPath, true);
            HttpResponse Response = HttpContext.Current.Response;
            FileInfo downfile = new FileInfo(zipFile);
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + sinfo.Folder + ".zip");
            Response.AddHeader("Content-Length", downfile.Length.ToString());
            Response.ContentType = "application/x-zip-compressed";
            Response.Flush();
            Response.TransmitFile(zipFile);
            Response.End();
        }
        public ResultData StyleImport()
        {
            string templateDir = ConfigurationManager.AppSettings["TemplateDir"].ToString();
            string staticDir = ConfigurationManager.AppSettings["StaticDir"].ToString();
            string styleDir = ConfigurationManager.AppSettings["StyleDir"].ToString();
            string uploadDir = ConfigurationManager.AppSettings["UploadDir"].ToString();


            string zipFilePath = staticDir + "/" + uploadDir + "/temp";
            var httpRequest = HttpContext.Current.Request;
            ResultData rdata = FileUpload(httpRequest, zipFilePath);
            if (rdata.Code != 0)
            {
                return rdata;
            }
            List<string> fs = rdata.Data as List<string>;
            string zipfile = fs[0];
            string fn = Path.GetFileNameWithoutExtension(zipfile);

            //模板目录
            string viewPath = "Views/" + templateDir + "/" + fn;
            string viewDir = HttpContext.Current.Server.MapPath("~/" + viewPath);
            //风格目录
            string stylePath = staticDir + "/" + styleDir + "/" + fn;
            string sDir = HttpContext.Current.Server.MapPath("~/" + stylePath);

            if (Directory.Exists(viewDir) || Directory.Exists(sDir))
            {
                return new ResultData
                {
                    Code = 1,
                    Msg = "文件夹已存在"
                };
            }
            try
            {
                ZipFile.ExtractToDirectory(zipfile, sDir);
                string tmplDir = HttpContext.Current.Server.MapPath("~/" + stylePath + "/template");
                //风格解压文件 中包含模板目录
                if (Directory.Exists(tmplDir))
                {
                    Directory.Move(tmplDir, viewDir);
                }
                //创建模板目录
                else
                {
                    Directory.CreateDirectory(viewDir);
                }
                GgcmsStyle sinfo = new GgcmsStyle
                {
                    Folder = fn,
                    StyleName = fn,
                    Descrip = fn,
                };
                return dbtool.Add(sinfo);
            }
            catch (Exception ex)
            {
                return new ResultData
                {
                    Code = 1,
                    Msg = ex.Message,
                    Data = ex
                };
            }
        }
    }
}