using GgcmsCSharp.ApiCtrls;
using GgcmsCSharp.Models;
using GgcmsCSharp.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;

namespace GgcmsCSharp.Models
{
    public class DataHelper
    {
        private GgcmsDB db = new GgcmsDB();
        public string Prefix { get; set; }
        public DataHelper()
        {
            Prefix = ConfigurationManager.AppSettings["LinkPrefix"].ToString();
        }
        //获取分类列表
        public List<GgcmsCategories> Categories()
        {
            List<GgcmsCategories> list = CacheHelper.GetCategorys(Prefix);
            return list;
        }
        //获取分类列表
        public List<GgcmsCategories> Categories(params int[] values)
        {
            List<GgcmsCategories> all = Categories();

            List<GgcmsCategories> list = new List<GgcmsCategories>();
            foreach(int cid in values)
            {
                var item = GgcmsCategories.GetCategoryById(cid,all);
                if (item != null)
                {
                    list.Add(item);
                }
            }
            return list;
        }
        //获取单个分类
        public GgcmsCategories Categories(int id)
        {
            List<GgcmsCategories> list = Categories();
            return GgcmsCategories.GetCategoryById(id, list);
        }
        //获取单个分类
        public GgcmsCategories Categories(string id)
        {
            List<GgcmsCategories> list = Categories();
            return GgcmsCategories.GetCategoryByKey(id, list);
        }
        //获取文章列表
        public GgcmsArticles Article(int id, int page = 1)
        {
            GgcmsArticles article = db.GgcmsArticles.Find(id);
            if (page > 1)
            {
                GgcmsArticlePages art_page = db.GgcmsArticlePages.Where(x => x.Article_Id == id && x.OrderId == page).FirstOrDefault();
                if (art_page != null)
                {
                    article.Content = art_page.Content;
                    article.PageTitle = art_page.Title;
                }
            }
            var list = from r in db.GgcmsAttachments
                       where r.Articles_Id == article.Id
                       select r;
            article.attachments = list.ToList();
            article.RedirectUrl = string.IsNullOrEmpty(article.RedirectUrl.Trim()) ? Prefix+"/Article/" + article.Id.ToString() : article.RedirectUrl;
            if (article.ExtModelId > 0)
            {
                article.ModuleInfo = ExtendModule.GetGgcmsModule(article.ExtModelId);
                article.ModuleData = ExtendModule.GetModuleToDict(article.Id, article.ModuleInfo);
            }
            return article;
        }
        public int GetCategoryTopId(GgcmsCategories info)
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
        public Pagination paginationInstance(int pnum = 1, int psize = 0)
        {
            Pagination pagination = new Pagination();
            pagination.pageSize = psize;
            pagination.page = pnum;
            return pagination;
        }
        public SearchResult<GgcmsFriendLinks> FriendLinks(string query, string columns = "", int pn = 1, int ps = 100, string orderby = "OrderId asc")
        {
            if (!query.ToLower().Contains("status") && !string.IsNullOrWhiteSpace(query))
            {
                query = query + " and Status = 1";
            }
            columns = string.IsNullOrWhiteSpace(columns) ? "Id,OrderId,Url,WebName,LogoImg,LinkType,RelationId,ExtAttrib" : columns;
            query = string.IsNullOrWhiteSpace(query) ? "Status = 1" : query;
            SearchParams rparams = new SearchParams
            {
                Columns = columns,
                QueryString = query,
                PageNum = pn,
                PageSize = ps,
                OrderBy = orderby
            };

            return GetRecords<GgcmsFriendLinks>(rparams);
        }
        public SearchResult<GgcmsAdverts> Adverts(string query, string columns = "", int pn = 1, int ps = 100, string orderby = "OrderId asc")
        {
            if (!query.ToLower().Contains("status")&& !string.IsNullOrWhiteSpace(query))
            {
                query = query + " and Status = 1";
            }
            columns = string.IsNullOrWhiteSpace(columns) ? "Id,Title,Url,Image,GroupKey,Content,OrderID" : columns;
            query = string.IsNullOrWhiteSpace(query) ? "Status = 1" : query;
            SearchParams rparams = new SearchParams
            {
                Columns = columns,
                QueryString = query,
                PageNum = pn,
                PageSize = ps,
                OrderBy = orderby
            };

            return GetRecords<GgcmsAdverts>(rparams);

        }
        public DataTable Views(int mid, SearchParams rparams)
        {
            return ExtendModule.ViewArticles(mid, rparams);
        }
        public DataTable Views(int mid,string query, string columns = "", int pn = 1, int ps = 100, string orderby="Id desc")
        {
            SearchParams rparams = new SearchParams
            {
                Columns = columns,
                QueryString = query,
                PageNum = pn,
                PageSize = ps,
                OrderBy = orderby
            };

            return Views(mid, rparams);
        }
        public SearchResult<GgcmsArticles> Articles(SearchParams rparams)
        {
            var result = GetRecords<GgcmsArticles>(rparams);
            List<GgcmsArticles> list = result.GetList();
            result.Records = new List<dynamic>();
            foreach (var ainfo in list)
            {
                GgcmsArticles ninfo = GgcmsArticles.Clone(ainfo);
                ninfo.RedirectUrl = string.IsNullOrEmpty(ninfo.RedirectUrl.Trim()) ? Prefix + "/Article/" + ninfo.Id.ToString() : ninfo.RedirectUrl;
                result.Records.Add(ninfo);
            }
            return result;
        }
        public SearchResult<GgcmsArticles> Articles(string query, string columns="", int pn = 1, int ps = 100, string orderby = "Id desc")
        {
            SearchParams rparams = new SearchParams
            {
                Columns = columns,
                QueryString = query,
                PageNum = pn,
                PageSize = ps,
                OrderBy = orderby
            };
            return Articles(rparams);
        }
        public SearchResult<GgcmsArticles> Articles(int[] ids, Pagination pagination)
        {

            SearchParams rparams = new SearchParams
            {
                Columns = "Author,Category_Id,CreateTime,Description,ExtModelId,Hits,Id,Keywords,MobileTmplName,PageTitle,RedirectUrl,Source,SourceUrl,StyleName,Title,TitleImg,TitleThumbnail,TmplName,ShowLevel,ShowType",
                QueryString = "@0.Contains(outerIt.Category_Id)",
                PageNum = pagination.page,
                PageSize = pagination.pageSize,
                OrderBy = "ShowLevel desc,Id desc",
                WhereParams= ids
            };
            return Articles(rparams);
        }
        public Dictionary<string, string> SysConfigs()
        {

            Dictionary<string, string> cfgs = CacheHelper.GetSysConfigs();
            cfgs["cfg_access_key"] = "";
            cfgs["cfg_secret_key"] = "";
            cfgs["cfg_bucket"] = "";
            return cfgs;
        }
        public SearchResult<GgcmsDictionaries> DictionaryList(string query, string columns = "", int pn = 1, int ps = 100, string orderby = "OrderID asc")
        {
            SearchParams rparams = new SearchParams
            {
                Columns = columns,
                QueryString = query,
                PageNum = pn,
                PageSize = ps,
                OrderBy = orderby
            };
            return DictionaryList(rparams);
        }
        public SearchResult<GgcmsDictionaries> DictionaryList(SearchParams rparams)
        {
            return GetRecords<GgcmsDictionaries>(rparams);
        }

        public SearchResult<T> GetRecords<T>(SearchParams sParams) where T : class
        {
            IQueryable list = db.Set(typeof(T));

            if (!string.IsNullOrWhiteSpace(sParams.QueryString))
            {
                if (sParams.WhereParams != null)
                {
                    Type t = sParams.WhereParams.GetType();

                    if (t.FullName.Contains("Linq.JArray"))
                    {
                        JArray array = sParams.WhereParams as JArray;

                        var ot = array.ToArray().FirstOrDefault().Type;
                        switch (ot)
                        {
                            case JTokenType.Integer:
                                list = list.Where(sParams.QueryString, array.ToArray().Select(x => x.Value<int>()).ToArray());
                                break;
                            case JTokenType.String:
                                list = list.Where(sParams.QueryString, array.ToArray().Select(x => x.Value<string>()).ToList());
                                break;

                        }
                    }
                    else if (t.FullName.Contains("System.Int32[]"))
                    {
                        list = list.Where(sParams.QueryString, sParams.WhereParams);
                    }
                }
                else
                {
                    list = list.Where(sParams.QueryString, null);
                }

            }
            if (!string.IsNullOrWhiteSpace(sParams.OrderBy))
            {
                list = list.OrderBy(sParams.OrderBy);
            }
            else
            {
                list = list.OrderBy("id desc");
            }
            SearchResult<T> result = new SearchResult<T>();
            result.Count = list.Count();
            if (sParams.PageSize > 0 && sParams.PageNum > 0)
            {
                int sikp = (sParams.PageNum - 1) * sParams.PageSize;
                list = list.Skip(sikp).Take(sParams.PageSize);
            }
            if (!string.IsNullOrEmpty(sParams.Columns))
            {
                list = list.Select("New(" + sParams.Columns + ")");
            }
            result.Records = new List<dynamic>();
            foreach (var item in list)
            {
                result.Records.Add(item);
            }
            return result;
        }
    }
}