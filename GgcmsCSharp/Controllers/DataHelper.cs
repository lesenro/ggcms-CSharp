using GgcmsCSharp.ApiCtrls;
using GgcmsCSharp.Models;
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
        public List<GgcmsFriendLink> FriendLinks(string query, string columns = "", int offset = 0, int limit = 100, string sortby = "OrderId", string order = "asc")
        {
            if (!query.ToLower().Contains("status") && !string.IsNullOrWhiteSpace(query))
            {
                query = query + ",Status:0";
            }
            RequestParams rparams = new RequestParams();
            rparams.offset = offset;
            rparams.limit = limit;
            rparams.columns = string.IsNullOrWhiteSpace(columns) ? "Id,OrderId,Url,WebName,LogoImg,LinkType,RelationId,ExtAttrib" : columns;
            rparams.sortby = sortby;
            rparams.order = order;
            rparams.query = string.IsNullOrWhiteSpace(query) ? "Status:0" : query;
            dbTools<GgcmsFriendLink> dbtool = new dbTools<GgcmsFriendLink>(rparams);
            ResultData result = dbtool.GetList(false);
            List<GgcmsFriendLink> fllist = new List<GgcmsFriendLink>();
            if (result.Code == 0)
            {
                IQueryable list = result.Data.List as IQueryable;
                foreach (var info in list)
                {
                    GgcmsFriendLink ninfo = GgcmsFriendLink.Clone(info);
                    fllist.Add(ninfo);
                }
            }
            return fllist;
        }
        public List<GgcmsAdverts> Adverts(string query, string columns = "", int offset = 0, int limit = 100, string sortby = "OrderID", string order = "asc")
        {
            if (!query.ToLower().Contains("status")&& !string.IsNullOrWhiteSpace(query))
            {
                query = query + ",Status:0";
            }
            RequestParams rparams = new RequestParams();
            rparams.offset = offset;
            rparams.limit = limit;
            rparams.columns = string.IsNullOrWhiteSpace(columns) ? "Id,Title,Url,Image,GroupKey,Content,OrderID" : columns;
            rparams.sortby = sortby;
            rparams.order = order;
            rparams.query = string.IsNullOrWhiteSpace(query) ? "Status:0" : query;
            dbTools<GgcmsAdverts> dbtool = new dbTools<GgcmsAdverts>(rparams);
            ResultData result = dbtool.GetList(false);
            List<GgcmsAdverts> adlist = new List<GgcmsAdverts>();
            if (result.Code == 0)
            {
                IQueryable list = result.Data.List as IQueryable;
                foreach (var info in list)
                {
                    GgcmsAdverts ninfo = GgcmsAdverts.Clone(info);
                    adlist.Add(ninfo);
                }
            }
            return adlist;
        }
        public ResultList<GgcmsArticle> Articles(RequestParams rparams)
        {
            dbTools<GgcmsArticle> dbtool = new dbTools<GgcmsArticle>(rparams);
            ResultData result = dbtool.GetList(false);
            ResultList<GgcmsArticle> rlist = new ResultList<GgcmsArticle>();
            if (result.Code == 0)
            {
                rlist.List = new List<GgcmsArticle>();
                IQueryable list = result.Data.List as IQueryable;
                foreach (var ainfo in list)
                {
                    GgcmsArticle ninfo = GgcmsArticle.Clone(ainfo);
                    ninfo.RedirectUrl = string.IsNullOrEmpty(ninfo.RedirectUrl.Trim()) ? Prefix+"/Article/" + ninfo.Id.ToString() : ninfo.RedirectUrl;
                    rlist.List.Add(ninfo);
                }
                rlist.Count = (int)result.Data.Count;
            }
            return rlist;
        }
        public List<GgcmsArticle> Articles(string query, string columns="", int offset=0,int limit=100,string sortby = "Id",string order="desc")
        {
            RequestParams rparams = new RequestParams();
            rparams.offset = offset;
            rparams.limit = limit;
            rparams.columns =string.IsNullOrWhiteSpace(columns)?"Author,Category_Id,CreateTime,Description,ExtModelId,Hits,Id,Keywords,MobileTmplName,PageTitle,RedirectUrl,Source,SourceUrl,StyleName,Title,TitleImg,TitleThumbnail,TmplName,ShowLevel,ShowType":columns;
            rparams.sortby = sortby;
            rparams.order = order;
            rparams.query = query;
            ResultList<GgcmsArticle> rlist = Articles(rparams);
            return rlist.List;
        }
        public List<GgcmsArticle> Articles(int[] ids, Pagination pagination)
        {
            RequestParams rparams = new RequestParams();
            rparams.offset = pagination.getSkip();
            rparams.limit = pagination.pageSize;
            rparams.columns = "Author,Category_Id,CreateTime,Description,ExtModelId,Hits,Id,Keywords,MobileTmplName,PageTitle,RedirectUrl,Source,SourceUrl,StyleName,Title,TitleImg,TitleThumbnail,TmplName,ShowLevel,ShowType";
            rparams.sortby = "Id";
            rparams.order = "desc";
            rparams.pagenum = pagination.page;
            rparams.query = "Category_Id.in:" + string.Join("|", ids);
            ResultList<GgcmsArticle> rlist = Articles(rparams);

            pagination.setMaxSize(rlist.Count);
            return rlist.List;
        }
    }
}