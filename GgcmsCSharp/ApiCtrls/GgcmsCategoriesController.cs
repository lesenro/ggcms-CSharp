using GgcmsCSharp.Models;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Linq;
using System;

namespace GgcmsCSharp.ApiCtrls
{
    public class GgcmsCategoriesController : ApiBaseController
    {

        // GET: api/GgcmsCategories
        [HttpGet]
        public ResultData GetList()
        {
            var reqParams = InitRequestParams<GgcmsCategory>();
            var result = dbHelper.GetList<GgcmsCategory>(reqParams);
            return new ResultData
            {
                Code = 0,
                Data = result,
                Msg = ""
            };
        }

        // GET: api/GgcmsCategories/5
        public ResultData GetInfo(int id)
        {
            var result = dbHelper.GetById<GgcmsCategory>(id);
            return new ResultData
            {
                Code = 0,
                Data = result,
                Msg = ""
            };
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
            return new ResultData
            {
                Code = 0,
                Data = dbHelper.Edit(category.Id, category),
                Msg = ""
            };
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
            UpFileClass.FileSave(category, category.files);
            CacheHelper.RemoveAllCache(CacheTypeNames.Categorys);
            return new ResultData
            {
                Code = 0,
                Msg = "",
                Data = dbHelper.Add(category)
            };
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
                dbHelper.dbCxt.GgcmsCategories
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

                result.Data = dbHelper.dbCxt.SaveChanges();
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
            return new ResultData
            {
                Code = 0,
                Msg = "",
                Data = dbHelper.Delete<GgcmsCategory>(id)
            };
        }
        [HttpGet]
        public ResultData MultDelete()
        {
            CacheHelper.RemoveAllCache(CacheTypeNames.Categorys);
            var reqParams = InitRequestParams<GgcmsCategory>();
            return new ResultData
            {
                Code = 0,
                Msg = "",
                Data = dbHelper.MultDelete<GgcmsCategory>(reqParams)
            };
        }


        public ResultData Exists(int id)
        {
            return new ResultData
            {
                Code = 0,
                Msg = "",
                Data = dbHelper.Exists<GgcmsCategory>(id)
            };
        }
    }
}