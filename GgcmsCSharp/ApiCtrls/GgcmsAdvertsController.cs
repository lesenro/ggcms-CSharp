using GgcmsCSharp.Models;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System;
using GgcmsCSharp.Controllers;
using System.Collections.Generic;
using System.Linq;

namespace GgcmsCSharp.ApiCtrls
{
    public class GgcmsAdvertsController : ApiBaseController
    {
        

        // GET: api/GgcmsCategories
        [HttpGet]
        public ResultData GetList()
        {
            var reqParams = InitRequestParams<GgcmsAdverts>();
            var result = dbHelper.GetList<GgcmsAdverts>(reqParams);
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
            var result = dbHelper.GetById<GgcmsAdverts>(id);
            return new ResultData
            {
                Code = 0,
                Data = result,
                Msg = ""
            };
        }

        // PUT: api/GgcmsCategories/5
        public ResultData Edit(GgcmsAdverts adverts)
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
            UpFileClass.FileSave(adverts, adverts.files);
            return new ResultData
            {
                Code = 0,
                Data = dbHelper.Edit(adverts.Id, adverts),
                Msg = ""
            };
        }

        // POST: api/GgcmsCategories
        public ResultData Add(GgcmsAdverts adverts)
        {
            if (!ModelState.IsValid)
            {
                return new ResultData
                {
                    Code = 3,
                    Msg = "",
                    Data = BadRequest(ModelState)
                }; 
            }
            adverts = UpFileClass.FileSave(adverts, adverts.files);
            return new ResultData
            {
                Code = 0,
                Msg = "",
                Data = dbHelper.Add(adverts)
            }; 
        }

        // DELETE: api/GgcmsCategories/5
        public ResultData Delete(int id)
        {
            return new ResultData
            {
                Code = 0,
                Msg = "",
                Data = dbHelper.Delete<GgcmsAdverts>(id)
            };

        }
        [HttpGet]
        public ResultData MultDelete()
        {
            var reqParams = InitRequestParams<GgcmsAdverts>();
            return new ResultData
            {
                Code = 0,
                Msg = "",
                Data = dbHelper.MultDelete<GgcmsAdverts>(reqParams)
            };
            
        }
        

        public ResultData Exists(int id)
        {
            return new ResultData
            {
                Code = 0,
                Msg = "",
                Data = dbHelper.Exists<GgcmsAdverts>(id)
            };
        }
    }
}