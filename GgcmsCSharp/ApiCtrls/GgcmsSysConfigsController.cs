using GgcmsCSharp.Models;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Linq;
using System;

namespace GgcmsCSharp.ApiCtrls
{
    public class GgcmsSysConfigsController : ApiController
    {
        private dbTools<GgcmsSysConfig> dbtool;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            this.dbtool = new dbTools<GgcmsSysConfig>(Request);
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
        public ResultData Edit(int id, GgcmsSysConfig GgcmsSysConfig)
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

            return dbtool.Edit(id, GgcmsSysConfig);
        }

        // POST: api/GgcmsCategories
        public ResultData Add(GgcmsSysConfig GgcmsSysConfig)
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
            return dbtool.Add(GgcmsSysConfig);
        }
        public ResultData SettingsSave(dynamic data)
        {

            ResultData result = new ResultData
            {
                Code = 0,
                Msg = "",
                Data = 0,
            };
            try
            {
                foreach(var file in data.files)
                {
                    foreach (var item in data.list)
                    {
                        if (item.CfgName.ToString() == file.propertyName.ToString())
                        {
                            item.CfgValue = UpFileClass.FileSave(file.filePath.ToString(), item.CfgValue.ToString(), (int)file.fileType);
                        }
                    }
                }
                dbtool.db.GgcmsSysConfigs
                   .ToList()
                   .ForEach(x =>
                   {
                       foreach (var item in data.list)
                       {
                           if (x.Id == (int)item.Id)
                           {
                               x.CfgValue = item.CfgValue.ToString();
                           }
                       }
                   });
                dbtool.db.SaveChanges();
                result.Data = data.list;
                CacheHelper.RemoveAllCache(CacheTypeNames.SysConfigs.ToString());
            }
            catch (Exception ex)
            {
                result.Data = ex;
                result.Code = 1;
                result.Msg = ex.Message;
            }
            return result;
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
            if (disposing&& dbtool!=null)
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