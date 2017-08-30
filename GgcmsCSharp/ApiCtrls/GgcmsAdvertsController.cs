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
    public class GgcmsAdvertsController : ApiController
    {
        private dbTools<GgcmsAdverts> dbtool;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            this.dbtool = new dbTools<GgcmsAdverts>(Request);
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

            return dbtool.Edit(adverts.Id, adverts);
        }

        // POST: api/GgcmsCategories
        public ResultData Add(GgcmsAdverts GgcmsAdverts)
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
            return dbtool.Add(GgcmsAdverts);
        }

        // DELETE: api/GgcmsCategories/5
        public ResultData Delete(int id)
        {
            return dbtool.Delete(id);
        }
        [HttpGet]
        public ResultData MultDelete()
        {
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