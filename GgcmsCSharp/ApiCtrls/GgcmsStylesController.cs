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
using GgcmsCSharp.Utils;
using System.Data.Entity;

namespace GgcmsCSharp.ApiCtrls
{
    public class GgcmsStylesController : ApiBaseController
    {
        private FileManager fman;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            this.fman = new FileManager();
        }
        // GET: api/GgcmsCategories
        [HttpGet]
        public IHttpActionResult GetList(string query)
        {
            string json = HttpUtility.UrlDecode(query);
            SearchParams sParams = Tools.JsonDeserialize<SearchParams>(json);

            return Ok(GetRecords<GgcmsStyles>(sParams));
        }

        // GET: api/GgcmsCategories/5
        public IHttpActionResult GetInfo(int id)
        {
            return Ok(Dbctx.GgcmsStyles.Find(id));

        }

        // PUT: api/GgcmsCategories/5
        public IHttpActionResult Edit(GgcmsStyles info)
        {
            if (Dbctx.GgcmsStyles.Where(x => x.Id == info.Id).Count() == 0)
            {
                return BadRequest("信息不存在");
            }
            //Dbctx.Entry(info).Property("goods_name").IsModified = true;
            var qlist = from r in Dbctx.GgcmsStyles
                        where r.Id == info.Id
                        select r;
            GgcmsStyles oldinfo = qlist.First();
            info.Folder = oldinfo.Folder;
            //Dbctx.GgcmsStyles.Attach(info);
            var ent = Dbctx.Entry(info);
            ent.State = EntityState.Modified;
            Dbctx.SaveChanges();
            ClearCache();
            return Ok(info);
        }

        // POST: api/GgcmsCategories
        public IHttpActionResult Add(GgcmsStyles styleInfo)
        {


            string staticDir = ConfigurationManager.AppSettings["StaticDir"].ToString();
            string styleDir = ConfigurationManager.AppSettings["StyleDir"].ToString();
            string root = "/" + staticDir + "/" + styleDir + "/" + styleInfo.Folder;
            string templateDir = ConfigurationManager.AppSettings["TemplateDir"].ToString();
            templateDir = "/Views/" + templateDir + "/" + styleInfo.Folder;

            string rootpath = HttpContext.Current.Server.MapPath("~" + root);
            string templatePath = HttpContext.Current.Server.MapPath("~" + templateDir);
            if (Directory.Exists(rootpath) || Directory.Exists(templatePath))
            {
                BadRequest("文件夹已经存在");
            }
            else
            {
                Directory.CreateDirectory(rootpath);
                Directory.CreateDirectory(templatePath);
            }
            var result = Dbctx.GgcmsStyles.Add(styleInfo);

            Dbctx.SaveChanges();
            ClearCache();
            return Ok(result);
        }

        // DELETE: api/GgcmsCategories/5
        public IHttpActionResult Delete(int id)
        {
            GgcmsStyles oldinfo = Dbctx.GgcmsStyles.Find(id);
            if (oldinfo == null)
            {
                return BadRequest("信息不存在");
            }

            //List<int> idlist = GetDeleteIds(oldinfo.ticket_key);

            //var query = Dbctx.ticket_information.Where(x => idlist.Contains(x.id));
            Dbctx.GgcmsStyles.Remove(oldinfo);
            Dbctx.SaveChanges();
            ClearCache();
            return Ok(oldinfo);
        }
        [HttpPost]
        public IHttpActionResult MultDelete(int[] ids)
        {


            string staticDir = ConfigurationManager.AppSettings["StaticDir"].ToString();
            string styleDir = ConfigurationManager.AppSettings["StyleDir"].ToString();
            styleDir = "/" + staticDir + "/" + styleDir + "/";
            string templateDir = ConfigurationManager.AppSettings["TemplateDir"].ToString();
            templateDir = "/Views/" + templateDir + "/";

            try
            {
                var query = Dbctx.GgcmsStyles.Where(x => ids.Contains(x.Id));
                foreach (var item in query.ToList())
                {
                    GgcmsStyles styleInfo = item as GgcmsStyles;
                    string stylePath = HttpContext.Current.Server.MapPath("~" + styleDir + styleInfo.Folder);
                    string templatePath = HttpContext.Current.Server.MapPath("~" + templateDir + styleInfo.Folder);
                    Directory.Delete(stylePath, true);
                    Directory.Delete(templatePath, true);
                }
                Dbctx.GgcmsStyles.RemoveRange(query);
                int c = Dbctx.SaveChanges();
                ClearCache();
                return Ok(c);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }


        public IHttpActionResult Exists(int id)
        {
            return Ok(Dbctx.GgcmsAdverts.Where(x => x.Id == id).Count() > 0);
        }
        [HttpGet]
        public IHttpActionResult GetStaticFile(int id, string path)
        {
            path = HttpUtility.UrlDecode(path);
            GgcmsStyles oldinfo = Dbctx.GgcmsStyles.Where(x=>x.Id==id).FirstOrDefault();
            string staticDir = ConfigurationManager.AppSettings["StaticDir"].ToString();
            string styleDir = ConfigurationManager.AppSettings["StyleDir"].ToString();
            string root = staticDir + "/" + styleDir + "/" + oldinfo.Folder + "/" + path;
            return Ok(fman.GetList(root));
        }
        [HttpGet]
        public IHttpActionResult GetStaticFileInfo(int id, string path)
        {
            path = HttpUtility.UrlDecode(path);
            GgcmsStyles sinfo = Dbctx.GgcmsStyles.Where(x => x.Id == id).FirstOrDefault();
            string staticDir = ConfigurationManager.AppSettings["StaticDir"].ToString();
            string styleDir = ConfigurationManager.AppSettings["StyleDir"].ToString();
            string root = staticDir + "/" + styleDir + "/" + sinfo.Folder + "/" + path;
            return Ok( fman.GetInfo(root));

        }
        public IHttpActionResult StaticFileSave(dynamic dataInfo)
        {
            int id = (int)dataInfo.id;
            string path = dataInfo.path.ToString();
            path = HttpUtility.UrlDecode(path);
            string content = dataInfo.content.ToString();
            GgcmsStyles sinfo = Dbctx.GgcmsStyles.Where(x => x.Id == id).FirstOrDefault();
            string staticDir = ConfigurationManager.AppSettings["StaticDir"].ToString();
            string styleDir = ConfigurationManager.AppSettings["StyleDir"].ToString();
            string fullname = staticDir + "/" + styleDir + "/" + sinfo.Folder + "/" + path;
            return Ok(fman.Save(fullname, content));

        }

        public IHttpActionResult StaticFileDelete(dynamic dataInfo)
        {
            int id = (int)dataInfo.id;
            string path = dataInfo.path.ToString();
            path = HttpUtility.UrlDecode(path);
            dynamic files = dataInfo.files;

            GgcmsStyles sinfo = Dbctx.GgcmsStyles.Where(x => x.Id == id).FirstOrDefault();
            string staticDir = ConfigurationManager.AppSettings["StaticDir"].ToString();
            string styleDir = ConfigurationManager.AppSettings["StyleDir"].ToString();
            string fullname = staticDir + "/" + styleDir + "/" + sinfo.Folder + "/" + path;
            return Ok( fman.Delete(fullname, files));

        }
        //新建文件夹
        public IHttpActionResult StaticFileNewDir(dynamic dataInfo)
        {
            int id = (int)dataInfo.id;
            string path = dataInfo.path.ToString();
            string newName = dataInfo.newName.ToString();
            path = HttpUtility.UrlDecode(path);

            GgcmsStyles sinfo = Dbctx.GgcmsStyles.Where(x => x.Id == id).FirstOrDefault();
            string staticDir = ConfigurationManager.AppSettings["StaticDir"].ToString();
            string styleDir = ConfigurationManager.AppSettings["StyleDir"].ToString();
            string fullname = staticDir + "/" + styleDir + "/" + sinfo.Folder + "/" + path + "/" + newName;
            return Ok( fman.NewDir(fullname));

        }
        //重命名
        public IHttpActionResult StaticFileReName(dynamic dataInfo)
        {
            int id = (int)dataInfo.id;
            string path = dataInfo.path.ToString();
            string newName = dataInfo.newName.ToString();
            string oldName = dataInfo.oldName.ToString();
            GgcmsStyles sinfo = Dbctx.GgcmsStyles.Where(x => x.Id == id).FirstOrDefault();
            string staticDir = ConfigurationManager.AppSettings["StaticDir"].ToString();
            string styleDir = ConfigurationManager.AppSettings["StyleDir"].ToString();
            string nname = staticDir + "/" + styleDir + "/" + sinfo.Folder + "/" + path + "/" + newName;
            string oname = staticDir + "/" + styleDir + "/" + sinfo.Folder + "/" + path + "/" + oldName;
            nname = HttpUtility.UrlDecode(nname);
            oname = HttpUtility.UrlDecode(oname);
            return Ok( fman.StaticFileReName(oname, nname));

        }
        //上传
        public IHttpActionResult StaticFileUpload()
        {
            var httpRequest = HttpContext.Current.Request;
            int id = int.Parse(httpRequest.Form["id"]);
            string path = httpRequest.Form["path"];
            path = HttpUtility.UrlDecode(path);
            GgcmsStyles sinfo = Dbctx.GgcmsStyles.Where(x => x.Id == id).FirstOrDefault();
            string staticDir = ConfigurationManager.AppSettings["StaticDir"].ToString();
            string styleDir = ConfigurationManager.AppSettings["StyleDir"].ToString();
            string fullname = staticDir + "/" + styleDir + "/" + sinfo.Folder + "/" + path;

            return Ok( FileUpload(httpRequest, fullname));

        }
        private ResultInfo FileUpload(HttpRequest httpRequest, string path)
        {
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
                            fn = Path.GetFileName(Tools.getRandString(8)) + ext;
                            filePath = HttpContext.Current.Server.MapPath("~/" + path + "/" + fn);
                        }
                        postedFile.SaveAs(filePath);
                        fs.Add(filePath);
                    }
                    return new ResultInfo {
                        Code = 0,
                        Msg = "上传成功",
                        Data = fs
                    };
                }
                return new ResultInfo
                {
                    Code = 1,
                    Msg = "上传文件为空",
                };
            }
            catch (Exception ex)
            {
                return new ResultInfo
                {
                    Code = 1,
                    Msg = ex.Message,
                };
            }

        }

        [HttpGet]
        public IHttpActionResult GetTemplateList(int id)
        {
            GgcmsStyles styleInfo = Dbctx.GgcmsStyles.Where(x => x.Id == id).FirstOrDefault();
            string templateDir = ConfigurationManager.AppSettings["TemplateDir"].ToString();
            templateDir = "Views/" + templateDir + "/" + styleInfo.Folder;
            return Ok( fman.GetList(templateDir));
        }
        [HttpGet]
        public IHttpActionResult GetTemplateInfo(int id, string filename)
        {
            filename = HttpUtility.UrlDecode(filename);
            GgcmsStyles styleInfo = Dbctx.GgcmsStyles.Where(x => x.Id == id).FirstOrDefault();
            string templateDir = ConfigurationManager.AppSettings["TemplateDir"].ToString();
            templateDir = "Views/" + templateDir + "/" + styleInfo.Folder + "/" + filename;

            return Ok( fman.GetInfo(templateDir));
        }
        public IHttpActionResult TemplateFileSave(dynamic dataInfo)
        {
            int id = (int)dataInfo.id;
            string filename = dataInfo.filename.ToString();
            string content = dataInfo.content.ToString();
            GgcmsStyles sinfo = Dbctx.GgcmsStyles.Where(x => x.Id == id).FirstOrDefault();
            string templateDir = ConfigurationManager.AppSettings["TemplateDir"].ToString();
            string fullname = "Views/" + templateDir + "/" + sinfo.Folder + "/" + filename;
            return Ok(fman.Save(fullname, content));
        }
        public IHttpActionResult TemplateFileDelete(dynamic dataInfo)
        {
            int id = (int)dataInfo.id;
            dynamic files = dataInfo.files;
            GgcmsStyles sinfo = Dbctx.GgcmsStyles.Where(x => x.Id == id).FirstOrDefault();
            string templateDir = ConfigurationManager.AppSettings["TemplateDir"].ToString();
            string fullname = "Views/" + templateDir + "/" + sinfo.Folder;
            return Ok(fman.Delete(fullname, files));

        }
        public IHttpActionResult TemplateFileUpload()
        {
            var httpRequest = HttpContext.Current.Request;
            int id = int.Parse(httpRequest.Form["id"]);
            GgcmsStyles sinfo = Dbctx.GgcmsStyles.Where(x => x.Id == id).FirstOrDefault();
            string templateDir = ConfigurationManager.AppSettings["TemplateDir"].ToString();
            string fullname = "Views/" + templateDir + "/" + sinfo.Folder;

            return Ok( FileUpload(httpRequest, fullname));

        }
        public IHttpActionResult TemplateFileReName(dynamic dataInfo)
        {
            int id = (int)dataInfo.id;
            string newName = dataInfo.newName.ToString();
            string oldName = dataInfo.oldName.ToString();
            GgcmsStyles sinfo = Dbctx.GgcmsStyles.Where(x => x.Id == id).FirstOrDefault();
            string templateDir = ConfigurationManager.AppSettings["TemplateDir"].ToString();

            string nname = "Views/" + templateDir + "/" + sinfo.Folder + "/" + newName;
            string oname = "Views/" + templateDir + "/" + sinfo.Folder + "/" + oldName;
            nname = HttpUtility.UrlDecode(nname);
            oname = HttpUtility.UrlDecode(oname);
            return Ok( fman.StaticFileReName(oname, nname));

        }
        [HttpGet]
        public void StyleExport(int id)
        {
            GgcmsStyles sinfo = Dbctx.GgcmsStyles.Where(x => x.Id == id).FirstOrDefault();
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
            string zipFilePath = staticDir + "/" + uploadDir + "/temp/" + Path.GetFileName(Tools.getRandString(8)) + ".zip";
            string zipFile = HttpContext.Current.Server.MapPath("~/" + zipFilePath);
            while (File.Exists(zipFile))
            {
                zipFilePath = staticDir + "/" + uploadDir + "/temp/" + Path.GetFileName(Tools.getRandString(8)) + ".zip";
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
        public IHttpActionResult StyleImport()
        {
            string templateDir = ConfigurationManager.AppSettings["TemplateDir"].ToString();
            string staticDir = ConfigurationManager.AppSettings["StaticDir"].ToString();
            string styleDir = ConfigurationManager.AppSettings["StyleDir"].ToString();
            string uploadDir = ConfigurationManager.AppSettings["UploadDir"].ToString();


            string zipFilePath = staticDir + "/" + uploadDir + "/temp";
            var httpRequest = HttpContext.Current.Request;
            ResultInfo rdata = FileUpload(httpRequest, zipFilePath);
            if (rdata.Code != 0)
            {
                return Ok( rdata);            }
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
                return BadRequest("文件夹已存在");
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
                GgcmsStyles sinfo = new GgcmsStyles
                {
                    Folder = fn,
                    StyleName = fn,
                    Descrip = fn,
                };
                sinfo= Dbctx.GgcmsStyles.Add(sinfo);
                return Ok(sinfo);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}