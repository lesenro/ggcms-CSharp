using GgcmsCSharp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace GgcmsCSharp.Utils
{
    public class GenerateStaticPages
    {
        private StaticTaskInfo taskInfo;
        private DataHelper dhlp;
        private HttpClient webclient;
        private string prefix;
        private HttpContext context;
        public GenerateStaticPages(StaticTaskInfo sinfo, HttpContext ctx)
        {
            taskInfo = sinfo;
            dhlp = new DataHelper();
            webclient = new HttpClient();
            prefix = ConfigurationManager.AppSettings["ServerBaseUrl"].ToString();
            context = ctx;
        }
        public void Run()
        {
            using (GgcmsDB db = new GgcmsDB())
            {
                if (taskInfo.All)
                {
                    GenerateHome();
                    GenerateCategories(db.GgcmsCategories.ToList());
                }
                else
                {
                    if (taskInfo.Categories.Count() > 0)
                    {
                        GenerateCategories(db.GgcmsCategories.Where(x => taskInfo.Categories.Contains(x.Id)).ToList());
                    }
                }
            }
        }
        private void GenerateHome()
        {
            string html = webclient.DownloadString(prefix);
            string svrpath = context.Server.MapPath("~");
            string fn = Path.GetFullPath(svrpath + "\\index.html");
            File.WriteAllText(fn, html, Encoding.UTF8);
        }

        private void GenerateCategories(List<GgcmsCategories> list)
        {
            string curl = prefix+ "/Category/";
            string svrpath = context.Server.MapPath("~");
            string dir = Path.GetFullPath(svrpath + "/category");
            var cfgs = dhlp.SysConfigs();
            int pageSize = Convert.ToInt32(cfgs["cfg_page_size"]);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            using (GgcmsDB db = new GgcmsDB())
            {
                foreach (var c in list)
                {
                    string html = webclient.DownloadString(curl + c.Id);
                    string fn = Path.GetFullPath(dir + "/" + (string.IsNullOrWhiteSpace(c.RouteKey) ? c.Id.ToString() : c.RouteKey.Trim()));
                    if (!Directory.Exists(fn))
                    {
                        Directory.CreateDirectory(fn);
                    }
                    fn = fn + "\\index.html";
                    File.WriteAllText(fn, html, Encoding.UTF8);
                    decimal count = db.GgcmsArticles.Where(x => x.Category_Id == c.Id).Count();
                    int psize = c.PageSize == 0 ? pageSize : c.PageSize;
                    int pagecount = Convert.ToInt32(Math.Ceiling(count / psize));
                    for(int p = 1; p < pagecount; p++)
                    {
                        int page = p + 1;
                        html = webclient.DownloadString(curl + c.Id + "/" + page.ToString());
                        fn = Path.GetFullPath(dir + "/" + (string.IsNullOrWhiteSpace(c.RouteKey) ? c.Id.ToString() : c.RouteKey.Trim()) + "/" + page.ToString());
                        if (!Directory.Exists(fn))
                        {
                            Directory.CreateDirectory(fn);
                        }
                        fn = fn + "\\index.html";
                        File.WriteAllText(fn, html, Encoding.UTF8);
                    }
                    GenerateArticles(c);
                }
            }
            
        }
        private void GenerateArticles(GgcmsCategories cinfo)
        {
            string aurl = prefix + "/Article/";
            string svrpath = context.Server.MapPath("~");
            string dir = Path.GetFullPath(svrpath + "/article");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using (GgcmsDB db=new GgcmsDB())
            {
                var list = db.GgcmsArticles.Where(x => x.Category_Id == cinfo.Id).ToList();
                foreach(var info in list)
                {
                    string html = webclient.DownloadString(aurl + info.Id);
                    string fn = Path.GetFullPath(dir + "/" + info.Id.ToString());
                    if (!Directory.Exists(fn))
                    {
                        Directory.CreateDirectory(fn);
                    }
                    fn = fn + "\\index.html";
                    File.WriteAllText(fn, html, Encoding.UTF8);
                    decimal count = info.pagesCount;
                    for (int p = 1; p < count; p++)
                    {
                        int page = p + 1;
                        html = webclient.DownloadString(aurl + info.Id + "/" + page.ToString());
                        fn = Path.GetFullPath(dir + "/" + info.Id.ToString() + "/" + page.ToString());
                        if (!Directory.Exists(fn))
                        {
                            Directory.CreateDirectory(fn);
                        }
                        fn = fn + "\\index.html";
                        File.WriteAllText(fn, html, Encoding.UTF8);
                    }
                }
            }
        }
        private void GenerateTopics(List<GgcmsTopics> list)
        {

        }
    }
}