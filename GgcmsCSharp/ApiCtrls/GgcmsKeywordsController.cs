using GgcmsCSharp.Models;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;

namespace GgcmsCSharp.ApiCtrls
{
    public class GgcmsKeywordsController : ApiBaseController
    {


        // GET: api/GgcmsCategories
        [HttpGet]
        public ResultData GetList()
        {
            var reqParams = InitRequestParams<GgcmsKeyword>();
            var result = dbHelper.GetList<GgcmsKeyword>(reqParams);
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
            var result = dbHelper.GetById<GgcmsKeyword>(id);
            return new ResultData
            {
                Code = 0,
                Data = result,
                Msg = ""
            };
        }

        // PUT: api/GgcmsCategories/5
        public ResultData Edit(int id, GgcmsKeyword GgcmsKeyword)
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

            return new ResultData
            {
                Code = 0,
                Data = dbHelper.Edit(GgcmsKeyword.Id, GgcmsKeyword),
                Msg = ""
            };
        }

        // POST: api/GgcmsCategories
        public ResultData Add(GgcmsKeyword GgcmsKeyword)
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
            return new ResultData
            {
                Code = 0,
                Msg = "",
                Data = dbHelper.Add(GgcmsKeyword)
            };
        }

        // DELETE: api/GgcmsCategories/5
        public ResultData Delete(int id)
        {
            return new ResultData
            {
                Code = 0,
                Msg = "",
                Data = dbHelper.Delete<GgcmsKeyword>(id)
            };
        }
        [HttpGet]
        public ResultData MultDelete()
        {
            var reqParams = InitRequestParams<GgcmsKeyword>();
            return new ResultData
            {
                Code = 0,
                Msg = "",
                Data = dbHelper.MultDelete<GgcmsKeyword>(reqParams)
            };
        }

        public ResultData Exists(int id)
        {
            return new ResultData
            {
                Code = 0,
                Msg = "",
                Data = dbHelper.Exists<GgcmsKeyword>(id)
            };
        }
    }
}