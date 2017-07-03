﻿using GgcmsCSharp.Models;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;

namespace GgcmsCSharp.ApiCtrls
{
    public class GgcmsArticlePagesController : ApiController
    {
        private dbTools<GgcmsArticlePage> dbtool;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            this.dbtool = new dbTools<GgcmsArticlePage>(Request);
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
        public ResultData Edit(int id, GgcmsArticlePage GgcmsArticlePage)
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

            return dbtool.Edit(id, GgcmsArticlePage);
        }

        // POST: api/GgcmsCategories
        public ResultData Add(GgcmsArticlePage GgcmsArticlePage)
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
            return dbtool.Add(GgcmsArticlePage);
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
            if (disposing)
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