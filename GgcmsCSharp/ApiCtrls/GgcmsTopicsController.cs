using GgcmsCSharp.Models;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;

namespace GgcmsCSharp.ApiCtrls
{
    public class GgcmsTopicsController : ApiBaseController
    {

        // GET: api/GgcmsCategories
        [HttpGet]
        public ResultData GetList()
        {
            var reqParams = InitRequestParams<GgcmsTopic>();
            var result = dbHelper.GetList<GgcmsTopic>(reqParams);
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
            var result = dbHelper.GetById<GgcmsTopic>(id);
            return new ResultData
            {
                Code = 0,
                Data = result,
                Msg = ""
            };
        }

        // PUT: api/GgcmsCategories/5
        public ResultData Edit(int id, GgcmsTopic topic)
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
                Data = dbHelper.Edit(topic.Id, topic),
                Msg = ""
            };
        }

        // POST: api/GgcmsCategories
        public ResultData Add(GgcmsTopic topic)
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
                Data = dbHelper.Add(topic)
            };
        }

        // DELETE: api/GgcmsCategories/5
        public ResultData Delete(int id)
        {
            return new ResultData
            {
                Code = 0,
                Msg = "",
                Data = dbHelper.Delete<GgcmsTopic>(id)
            };
        }
        [HttpGet]
        public ResultData MultDelete()
        {
            var reqParams = InitRequestParams<GgcmsTopic>();
            return new ResultData
            {
                Code = 0,
                Msg = "",
                Data = dbHelper.MultDelete<GgcmsTopic>(reqParams)
            };
        }


        public ResultData Exists(int id)
        {
            return new ResultData
            {
                Code = 0,
                Msg = "",
                Data = dbHelper.Exists<GgcmsTopic>(id)
            };
        }
    }
}