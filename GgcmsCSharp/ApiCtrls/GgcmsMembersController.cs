﻿using GgcmsCSharp.Models;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;

namespace GgcmsCSharp.ApiCtrls
{
    public class GgcmsMembersController : ApiController
    {
        private dbTools<GgcmsMember> dbtool;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            this.dbtool = new dbTools<GgcmsMember>(Request);
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
        public ResultData Edit(int id, GgcmsMember GgcmsMember)
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

            return dbtool.Edit(id, GgcmsMember);
        }

        // POST: api/GgcmsCategories
        public ResultData Add(GgcmsMember GgcmsMember)
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
            return dbtool.Add(GgcmsMember);
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