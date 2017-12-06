using GgcmsCSharp.ApiCtrls;
using GgcmsCSharp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GgcmsCSharp.ApiCtrls
{
    public enum CacheTypeNames
    {
        SysConfigs, Categorys, PagesCache
    }
    public class CacheHelper
    {
        public static Dictionary<string, string> GetSysConfigs()
        {
            string cacheName = CacheTypeNames.SysConfigs.ToString();
            Dictionary<string, string> cfgs = new Dictionary<string, string>();
            object obj = GetCache(cacheName);
            if (obj == null)
            {
                GgcmsDB db = new GgcmsDB();
                var tmps = from r in db.GgcmsSysConfigs
                           select new { r.Id, r.CfgName, r.CfgValue };
                foreach (var item in tmps)
                {
                    cfgs.Add(item.CfgName, item.CfgValue);
                }
                SetCache(cacheName, cfgs);
            }
            else
            {
                cfgs = obj as Dictionary<string, string>;
            }
            return cfgs;
        }
        public static List<GgcmsCategory> GetCategorys(string prefix="")
        {
            string cacheName = CacheTypeNames.Categorys.ToString();
            List<GgcmsCategory> categorys = new List<GgcmsCategory>();
            object obj = GetCache(cacheName);
            if (obj == null)
            {
                GgcmsDB db = new GgcmsDB();
                var tmps = (from r in db.GgcmsCategories
                            where r.CategoryType == 0
                            orderby r.OrderId ascending
                            select r).ToList();
                categorys = GgcmsCategory.GetCategoryList(0, tmps as List<GgcmsCategory>, prefix);
                SetCache(cacheName, categorys);
            }
            else
            {
                categorys = obj as List<GgcmsCategory>;
            }
            return categorys;
        }
        public static List<string> GetPages()
        {
            string cacheName = CacheTypeNames.PagesCache.ToString();
            List<string> pages = new List<string>();
            object obj = GetCache(cacheName);
            if (obj == null)
            {
                pages = new List<string>();
                SetCache(cacheName, pages);
            }
            else
            {
                pages = obj as List<string>;
            }
            return pages;
        }
        public static bool SetPages(string path)
        {
            string cacheName = CacheTypeNames.PagesCache.ToString();
            List<string> pages = GetPages();
            if (!pages.Contains(path))
            {
                pages.Add(path);
                SetCache(cacheName, pages);
                return true;
            }
            return false;
        }
        /// <summary>  
        /// 获取数据缓存  
        /// </summary>  
        /// <param name="CacheKey">键</param>  
        public static object GetCache(string CacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[CacheKey];
        }

        /// <summary>  
        /// 设置数据缓存  
        /// </summary>  
        public static void SetCache(string CacheKey, object objObject)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject);
        }

        /// <summary>  
        /// 设置数据缓存  
        /// </summary>  
        public static void SetCache(string CacheKey, object objObject, TimeSpan Timeout)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject, null, DateTime.MaxValue, Timeout, System.Web.Caching.CacheItemPriority.NotRemovable, null);
        }

        /// <summary>  
        /// 设置数据缓存  
        /// </summary>  
        public static void SetCache(string CacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject, null, absoluteExpiration, slidingExpiration);
        }

        /// <summary>  
        /// 移除指定数据缓存  
        /// </summary>  
        public static void RemoveAllCache(string CacheKey)
        {
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            _cache.Remove(CacheKey);
        }
        public static void RemoveAllCache(CacheTypeNames ctype)
        {
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            _cache.Remove(ctype.ToString());
        }
        /// <summary>  
        /// 移除全部缓存  
        /// </summary>  
        public static void RemoveAllCache()
        {
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                _cache.Remove(CacheEnum.Key.ToString());
            }
        }

    }
}