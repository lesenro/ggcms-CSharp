using GgcmsCSharp.ApiCtrls;
using GgcmsCSharp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace GgcmsCSharp
{
    public class WebApiCacheAttribute : ActionFilterAttribute
    {
        public DateTime absoluteExpiration { get; set; }
        public int slidingExpiration { get; set; }
        public bool needSession { get; set; }
        public int clientCacheExpiration { get; set; }
        /// <summary>
        /// 定义缓存
        /// </summary>
        /// <param name="absolute">绝对过期时间</param>
        /// <param name="sliding">滚动过期时间</param>
        /// <param name="ndSession">是否需要判断session</param>
        public WebApiCacheAttribute(DateTime absolute, int slidingMinutes, int clientExpiration = 600, bool ndSession = false)
        {
            absoluteExpiration = absolute;
            slidingExpiration = slidingMinutes;
            needSession = ndSession;
            clientCacheExpiration = clientExpiration;
        }
        public WebApiCacheAttribute(DateTime absolute)
            : this(absolute, 0)
        {
        }

        public WebApiCacheAttribute(int slidingMinutes)
            : this(System.Web.Caching.Cache.NoAbsoluteExpiration, slidingMinutes)
        {
        }
        public WebApiCacheAttribute()
            : this(DateTime.Now.AddMinutes(20d), 0)

        {
        }

        public WebApiCacheAttribute(string cfgname)
        {

        }
        #region 获取缓存
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!getCache(actionContext))
            {
                base.OnActionExecuting(actionContext);
            }
        }

        //public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        //{

        //    getCache(actionContext);
        //    return base.OnActionExecutingAsync(actionContext, cancellationToken);
        //}

        private bool getCache(HttpActionContext actionContext)
        {
            // 若无缓存,则直接返回

            string cacheKey = getCacheKey(actionContext);
            var obj = CacheHelper.GetCache(cacheKey);
            if (obj == null)
            {
                return false;
            }
            byte[] content = obj as byte[];


            // ****************************************   使用缓存  *********************************************
            /*
            // 获取缓存中的etag
            var etag = CacheHelper.GetCache(cacheKey + ":etag").ToString();
            // 若etag没有被改变，则返回304，
            if (actionContext.Request.Headers.IfNoneMatch.Any(x => x.Tag == etag))  //IfNoneMatch为空时，返回的是一个集合对象
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.NotModified); // 响应对象还没有生成，需要先生成 // 304 not modified
                //actionContext.Response.Headers.CacheControl = GetDefaultCacheControl();
                actionContext.Response.Headers.ETag = new EntityTagHeaderValue(etag);
            }
            else //从缓存中返回响应
            {
                actionContext.Response = actionContext.Request.CreateResponse(); // 响应对象还没有生成，需要先生成
                // 设置协商缓存：etag
                actionContext.Response.Headers.ETag = new EntityTagHeaderValue(etag); //用缓存中的值（为最新的）更新它
                // 设置user agent的本地缓存
               // actionContext.Response.Headers.CacheControl = GetDefaultCacheControl();

                // 从缓存中取中响应内容
                actionContext.Response.Content = new ByteArrayContent(content);
            }*/
            actionContext.Response = actionContext.Request.CreateResponse(); // 响应对象还没有生成，需要先生成
            // 从缓存中取中响应内容
            actionContext.Response.Content = new ByteArrayContent(content);
            return true;
        }

        #endregion
        #region 设置缓存

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            setCache(actionExecutedContext);
            base.OnActionExecuted(actionExecutedContext);
        }

        //public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        //{
        //    setCache(actionExecutedContext);
        //    return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        //}

        private async void setCache(HttpActionExecutedContext actionExecutedContext)
        {


            // 响应对象已经生成，可以直接调用   actionExecutedContext.Response

            // 设置协商缓存：etag
            //EntityTagHeaderValue etag = new EntityTagHeaderValue("\"" + Guid.NewGuid().ToString() + "\"");  // 根据http协议， ETag的值必须用引号包含起来
            //actionExecutedContext.Response.Headers.ETag = etag;
            // 设置user agent的本地缓存
            //actionExecutedContext.Response.Headers.CacheControl = GetDefaultCacheControl();

            // actionExecutedContext.Response.Headers.Remove("Content-Length"); // 改变了值，它会发生变化。删除它的话，后续的程序会自动地再计算

            // 保存到缓存
            string cacheKey = getCacheKey(actionExecutedContext.ActionContext);
            if (actionExecutedContext.Response == null)
            {
                return;
            }
            var contentBytes = await actionExecutedContext.Response.Content.ReadAsByteArrayAsync();
            //CacheHelper.SetCache(cacheKey + ":etag", etag.Tag, absoluteExpiration, slidingExpiration);
            TimeSpan timeSpan = TimeSpan.Zero;
            if (slidingExpiration > 0)
            {
                timeSpan = TimeSpan.FromMinutes(slidingExpiration);
            }
            CacheHelper.SetCache(cacheKey, contentBytes, absoluteExpiration, timeSpan);
        }

        #endregion
        /// <summary>
        /// 默认的用户代理本地缓存配置，10分钟的相对过期时间
        /// 对应响应header中的 Cache-Control
        /// 这里设置里面的子项max-age。如：Cache-Control: max-age=3600
        /// </summary>
        /// <returns></returns>
        //private CacheControlHeaderValue GetDefaultCacheControl()
        //{
        //    CacheControlHeaderValue control = new CacheControlHeaderValue();
        //    control.MaxAge = TimeSpan.FromSeconds(clientCacheExpiration);  // 它对应响应头中的 cacheControl :max-age=10项

        //    return control;
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        private string getCacheKey(HttpActionContext actionContext)
        {
            string cacheKey = null;

            cacheKey = actionContext.Request.RequestUri.Query; // 路径和查询部分,如： /api/values/1?ee=33&oo=5

            if (needSession)
            {
                cacheKey = cacheKey + HttpContext.Current.Session.SessionID;
            }

            return actionContext.Request.RequestUri.LocalPath + Tools.getMd5Hash(cacheKey);
        }
    }
}