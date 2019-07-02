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
        SysConfigs, Categorys, PagesCache,Tasks
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
                var tmps = from r in db.GgcmsDictionaries
                           where r.GroupKey== "system_configs"
                           select new { r.Id, r.DictKey, r.DictValue };
                foreach (var item in tmps)
                {
                    cfgs.Add(item.DictKey, item.DictValue);
                }
                SetCache(cacheName, cfgs);
            }
            else
            {
                cfgs = obj as Dictionary<string, string>;
            }
            return cfgs;
        }
        public static List<GgcmsCategories> GetCategorys(string prefix="")
        {
            string cacheName = CacheTypeNames.Categorys.ToString();
            List<GgcmsCategories> categorys = new List<GgcmsCategories>();
            object obj = GetCache(cacheName);
            if (obj == null)
            {
                GgcmsDB db = new GgcmsDB();
                var tmps = (from r in db.GgcmsCategories
                            where r.CategoryType == 0
                            orderby r.OrderId ascending
                            select r).ToList();
                categorys = GgcmsCategories.GetCategoryList(0, tmps as List<GgcmsCategories>, prefix);
                SetCache(cacheName, categorys);
            }
            else
            {
                categorys = obj as List<GgcmsCategories>;
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
        public static List<GgcmsTasks> GetTasks()
        {
            string cacheName = CacheTypeNames.Tasks.ToString();
            List<GgcmsTasks> tasks = new List<GgcmsTasks>();
            object obj = GetCache(cacheName);
            if (obj == null)
            {
                GgcmsDB db = new GgcmsDB();
                tasks = db.GgcmsTasks.Where(x => x.Switch == 1).ToList();
                SetCache(cacheName, tasks, new TimeSpan(0, 20, 0));
            }
            else
            {
                tasks = obj as List<GgcmsTasks>;
            }
            return tasks;
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
        public static void RemoveAllCache(string key)
        {
            List<string> cacheKeys = new List<string>();
            var cacheEnum = HttpRuntime.Cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                if (string.IsNullOrEmpty(key))
                {
                    cacheKeys.Add(cacheEnum.Key.ToString());
                }
                else if (cacheEnum.Key.ToString().StartsWith(key))
                {
                    cacheKeys.Add(cacheEnum.Key.ToString());
                }
            }
            foreach (string cacheKey in cacheKeys)
            {
                HttpRuntime.Cache.Remove(cacheKey);
            }
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