using GgcmsCSharp.Models;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Linq;
using System;

namespace GgcmsCSharp.ApiCtrls
{
    public class GgcmsCategoriesController : ApiController
    {
        private dbTools<GgcmsCategory> dbtool;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            this.dbtool = new dbTools<GgcmsCategory>(Request);
        }
        // GET: api/GgcmsCategories
        [HttpGet]
        public ResultData GetList()
        {
            return dbtool.GetList(); 
        }

        // GET: api/GgcmsCategories/5
        public ResultData GetInfo(int id)
        {
            return dbtool.GetById(id);
        }

        // PUT: api/GgcmsCategories/5
        public ResultData Edit(GgcmsCategory category)
        {

            if (!ModelState.IsValid)
            {
                ResultData result = new ResultData
                {
                    Code = 3,
                    Msg = "",
                    Data = BadRequest(ModelState)
                };
                return result;
            }
            UpFileClass.FileSave<GgcmsCategory>(category, category.files);
            CacheHelper.RemoveAllCache(CacheTypeNames.Categorys);
            return dbtool.Edit(category.Id, category);
        }

        // POST: api/GgcmsCategories
        public ResultData Add(GgcmsCategory category)
        {
            if (!ModelState.IsValid)
            {
                ResultData result = new ResultData
                {
                    Code = 3,
                    Msg = "",
                    Data = BadRequest(ModelState)
                };
                return result;
            }
            UpFileClass.FileSave<GgcmsCategory>(category, category.files);
            CacheHelper.RemoveAllCache(CacheTypeNames.Categorys);
            return dbtool.Add(category);
        }
        public ResultData CategorySortSave(dynamic[] list)
        {

            ResultData result = new ResultData
            {
                Code = 0,
                Msg = "",
                Data = 0,
                };
            try {
                dbtool.db.GgcmsCategories
                   .ToList()
                   .ForEach(x => {
                       foreach (var item in list)
                       {
                           if (x.Id == (int)item.Id)
                           {
                               x.OrderId = (int)item.OrderId;
                               x.ParentId = (int)item.ParentId;
                           }
                       }
                   });

                result.Data = dbtool.db.SaveChanges();
            }catch(Exception ex)
            {
                result.Data = ex;
                result.Code = 1;
                result.Msg = ex.Message;
            }
            CacheHelper.RemoveAllCache(CacheTypeNames.Categorys);

            return result;
        }
        // DELETE: api/GgcmsCategories/5
        public ResultData Delete(int id)
        {
            CacheHelper.RemoveAllCache(CacheTypeNames.Categorys);
            return dbtool.Delete(id);
        }
        [HttpGet]
        public ResultData MultDelete()
        {
            CacheHelper.RemoveAllCache(CacheTypeNames.Categorys);
            return dbtool.MultDelete();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && dbtool != null)
            {
                dbtool.Dispose(true);
            }
            base.Dispose(disposing);
        }

        public ResultData Exists(int id)
        {
            return dbtool.Exists(id);
        }
    }
}