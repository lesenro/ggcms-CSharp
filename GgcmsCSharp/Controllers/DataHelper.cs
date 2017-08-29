using GgcmsCSharp.ApiCtrls;
using GgcmsCSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GgcmsCSharp.Controllers
{
    public class DataHelper
    {
        private GgcmsDB db = new GgcmsDB();
        //获取分类列表
        public List<GgcmsCategory> Categories()
        {
            List<GgcmsCategory> list = CacheHelper.GetCategorys();
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
            article.RedirectUrl = string.IsNullOrEmpty(article.RedirectUrl.Trim()) ? "/Article/" + article.Id.ToString() : article.RedirectUrl;
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
                    ninfo.RedirectUrl = string.IsNullOrEmpty(ninfo.RedirectUrl.Trim()) ? "/Article/" + ninfo.Id.ToString() : ninfo.RedirectUrl;
                    rlist.List.Add(ninfo);
                }
                rlist.Count = (int)result.Data.Count;
            }
            return rlist;
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