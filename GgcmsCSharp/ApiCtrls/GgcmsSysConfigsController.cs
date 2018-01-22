using GgcmsCSharp.Models;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Linq;
using System;

namespace GgcmsCSharp.ApiCtrls
{
    public class GgcmsSysConfigsController : ApiBaseController
    {

        // GET: api/GgcmsCategories
        [HttpGet]
        public ResultData GetList()
        {
            var reqParams = InitRequestParams<GgcmsSysConfig>();
            var result = dbHelper.GetList<GgcmsSysConfig>(reqParams);
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
            var result = dbHelper.GetById<GgcmsSysConfig>(id);
            return new ResultData
            {
                Code = 0,
                Data = result,
                Msg = ""
            };
        }

        // PUT: api/GgcmsCategories/5
        public ResultData Edit(int id, GgcmsSysConfig config)
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
                Data = dbHelper.Edit(config.Id, config),
                Msg = ""
            };
        }

        // POST: api/GgcmsCategories
        public ResultData Add(GgcmsSysConfig config)
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
                Data = dbHelper.Add(config)
            };
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
                dbHelper.dbCxt.GgcmsSysConfigs
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
                dbHelper.dbCxt.SaveChanges();
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
            return new ResultData
            {
                Code = 0,
                Msg = "",
                Data = dbHelper.Delete<GgcmsSysConfig>(id)
            };
        }
        [HttpGet]
        public ResultData MultDelete()
        {
            var reqParams = InitRequestParams<GgcmsSysConfig>();
            return new ResultData
            {
                Code = 0,
                Msg = "",
                Data = dbHelper.MultDelete<GgcmsSysConfig>(reqParams)
            };
        }

        public ResultData Exists(int id)
        {
            return new ResultData
            {
                Code = 0,
                Msg = "",
                Data = dbHelper.Exists<GgcmsSysConfig>(id)
            };
        }
    }
}