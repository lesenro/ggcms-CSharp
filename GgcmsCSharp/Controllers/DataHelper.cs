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
        public List<GgcmsArticle> Articles(int[] ids, Pagination pagination, string title = "", int stype = -1, int slevel = -1, int order = 0)
        {
            var list = from c in db.GgcmsArticles
                       select new
                       {
                           c.Author,
                           c.Category_Id,
                           c.CreateTime,
                           c.Description,
                           c.ExtModelId,
                           c.Hits,
                           c.Id,
                           c.Keywords,
                           c.MobileTmplName,
                           c.PageTitle,
                           c.RedirectUrl,
                           c.Source,
                           c.SourceUrl,
                           c.StyleName,
                           c.Title,
                           c.TitleImg,
                           c.TitleThumbnail,
                           c.TmplName,
                           c.ShowLevel,
                           c.ShowType
                       };
            //排序
            if (order == 0)
            {
                list = list.OrderByDescending(x => x.Id);
            }
            else if (order == 1)
            {
                list = list.OrderByDescending(x => x.ShowLevel);
            }
            //分类id
            if (ids.Length > 0)
            {
                list = list.Where(x => ids.Contains(x.Category_Id));
            }

            //显示类型
            if (stype != -1)
            {
                list = list.Where(x => x.ShowType >= stype);
            }
            //名称过滤
            if (string.IsNullOrEmpty(title.Trim()))
            {
                list = list.Where(x => x.Title.Contains(title));
            }
            int count = list.Count();
            pagination.setMaxSize(count);
            list.Take(pagination.pageSize).Skip(pagination.getSkip());
            
            List<GgcmsArticle> articles = new List<GgcmsArticle>();

            foreach (var c in list)
            {
                var info = new GgcmsArticle
                {
                    Author = c.Author,
                    Category_Id = c.Category_Id,
                    CreateTime = c.CreateTime,
                    Description = c.Description,
                    ExtModelId = c.ExtModelId,
                    Hits = c.Hits,
                    Id = c.Id,
                    Keywords = c.Keywords,
                    MobileTmplName = c.MobileTmplName,
                    PageTitle = c.PageTitle,
                    RedirectUrl = string.IsNullOrEmpty(c.RedirectUrl.Trim()) ? "/Article/" + c.Id.ToString() : c.RedirectUrl,
                    Source = c.Source,
                    SourceUrl = c.SourceUrl,
                    StyleName = c.StyleName,
                    Title = c.Title,
                    TitleImg = c.TitleImg,
                    TitleThumbnail = c.TitleThumbnail,
                    TmplName = c.TmplName,
                    ShowLevel = c.ShowLevel,
                    ShowType = c.ShowType

                };
                articles.Add(info);
            }
            return articles;
        }
    }
}