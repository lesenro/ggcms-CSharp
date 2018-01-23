using GgcmsCSharp.ApiCtrls;
using GgcmsCSharp.Models;
using GgcmsCSharp.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace GgcmsCSharp.Controllers
{
    public class DataHelper
    {
        private GgcmsDB db = new GgcmsDB();
        public string Prefix { get; set; }
        public DataHelper()
        {
            Prefix = ConfigurationManager.AppSettings["UploadPrefix"].ToString();
        }
        //获取分类列表
        public List<GgcmsCategory> Categories()
        {
            List<GgcmsCategory> list = CacheHelper.GetCategorys(Prefix);
            return list;
        }
        //获取单个分类
        public GgcmsCategory Categories(int id)
        {
            List<GgcmsCategory> list = Categories();
            return GgcmsCategory.GetCategoryById(id, list);
        }
        //获取文章列表
        public GgcmsArticle Article(int id)
        {
            GgcmsArticle article = db.GgcmsArticles.Find(id);
            var list = from r in db.GgcmsAttachments
                       where r.Articles_Id == article.Id
                       select r;
            article.attachments = list.ToList();
            article.RedirectUrl = string.IsNullOrEmpty(article.RedirectUrl.Trim()) ? Prefix+"/Article/" + article.Id.ToString() : article.RedirectUrl;
            return article;
        }
        public int GetCategoryTopId(GgcmsCategory info)
        {
            if (info.ParentId == 0)
            {
                return info.Id;
            }
            else
            {
                return GetCategoryTopId(Categories(info.ParentId));
            }
        }
        public Pagination paginationInstance(int pnum = 1, int psize = 10)
        {
            Pagination pagination = new Pagination();
            pagination.pageSize = psize;
            pagination.page = pnum;
            return pagination;
        }
        public List<dynamic> FriendLinks(string query, string columns = "", int offset = 0, int limit = 100, string sortby = "OrderId", string order = "asc")
        {
            if (!query.ToLower().Contains("status") && !string.IsNullOrWhiteSpace(query))
            {
                query = query + ",Status:0";
            }
            columns = string.IsNullOrWhiteSpace(columns) ? "Id,OrderId,Url,WebName,LogoImg,LinkType,RelationId,ExtAttrib" : columns;
            query = string.IsNullOrWhiteSpace(query) ? "Status:0" : query;
            RequestParams rparams = RequestParams.GetRequestParams<GgcmsDB, GgcmsFriendLink>(columns, limit, offset, 1, order, sortby, query);
            

            DataBaseHelper<GgcmsDB> dbHelper = new DataBaseHelper<GgcmsDB>(new GgcmsDB());
            var result = dbHelper.GetList<GgcmsFriendLink>(rparams);
            return result.ToList();
        }
        public List<dynamic> Adverts(string query, string columns = "", int offset = 0, int limit = 100, string sortby = "OrderID", string order = "asc")
        {
            if (!query.ToLower().Contains("status")&& !string.IsNullOrWhiteSpace(query))
            {
                query = query + ",Status:0";
            }
            columns = string.IsNullOrWhiteSpace(columns) ? "Id,Title,Url,Image,GroupKey,Content,OrderID" : columns;
            query = string.IsNullOrWhiteSpace(query) ? "Status:0" : query;
            RequestParams rparams = RequestParams.GetRequestParams<GgcmsDB, GgcmsAdverts>(columns, limit, offset, 1, order, sortby, query);

            DataBaseHelper<GgcmsDB> dbHelper = new DataBaseHelper<GgcmsDB>(new GgcmsDB());

            var result = dbHelper.GetList<GgcmsAdverts>(rparams);
            return result.ToList();

        }
        public ListResult Articles(RequestParams rparams)
        {
            DataBaseHelper<GgcmsDB> dbHelper = new DataBaseHelper<GgcmsDB>(new GgcmsDB());
            rparams = RequestParams.GetRequestParams<GgcmsDB, GgcmsArticle>(rparams.columns, rparams.limit, rparams.offset, 1, rparams.order, rparams.sortby, rparams.query);
            return dbHelper.GetList<GgcmsArticle>(rparams);
        }
        public List<dynamic> Articles(string query, string columns="", int offset=0,int limit=100,string sortby = "Id",string order="desc")
        {
            RequestParams rparams = new RequestParams();
            rparams.offset = offset;
            rparams.limit = limit;
            rparams.columns =string.IsNullOrWhiteSpace(columns)?"Author,Category_Id,CreateTime,Description,ExtModelId,Hits,Id,Keywords,MobileTmplName,PageTitle,RedirectUrl,Source,SourceUrl,StyleName,Title,TitleImg,TitleThumbnail,TmplName,ShowLevel,ShowType":columns;
            rparams.sortby = sortby;
            rparams.order = order;
            rparams.query = query;
            ListResult rlist = Articles(rparams);

            return rlist.ToList();
        }
        public List<dynamic> Articles(int[] ids, Pagination pagination)
        {
            RequestParams rparams = new RequestParams();
            rparams.offset = pagination.getSkip();
            rparams.limit = pagination.pageSize;
            rparams.columns = "Author,Category_Id,CreateTime,Description,ExtModelId,Hits,Id,Keywords,MobileTmplName,PageTitle,RedirectUrl,Source,SourceUrl,StyleName,Title,TitleImg,TitleThumbnail,TmplName,ShowLevel,ShowType";
            rparams.sortby = "Id";
            rparams.order = "desc";
            rparams.pagenum = pagination.page;
            rparams.query = "Category_Id.in:" + string.Join("|", ids);
            ListResult rlist = Articles(rparams);

            pagination.setMaxSize(rlist.Count);
            return rlist.ToList();
        }
    }
}