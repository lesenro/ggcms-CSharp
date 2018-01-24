using GgcmsCSharp.Models;
using System.Collections.Generic;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Linq;
using System;
using System.Data.Entity;

namespace GgcmsCSharp.ApiCtrls
{
    public class GgcmsModulesController : ApiBaseController
    {

        // GET: api/GgcmsCategories
        [HttpGet]
        public ResultData GetList()
        {
            var reqParams = InitRequestParams<GgcmsModule>();
            var result = dbHelper.GetList<GgcmsModule>(reqParams);
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

            ResultData result = new ResultData
            {
                Code = 0,
                Msg = ""
            };
            GgcmsModule module = ExtendModule.GetGgcmsModule(id);
            if (module != null)
            {
                result.Data = module;
            }
            else
            {
                result.Code = 1;
                result.Msg = "not found";
            }
            
            return result;
        }
        
        // PUT: api/GgcmsCategories/5
        public ResultData Edit(GgcmsModule module)
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
            if (module.Columns != null)
            {
                GgcmsModule oldModule = ExtendModule.GetGgcmsModule(module.Id);
                module.TableName = oldModule.TableName;
                module.ViewName = oldModule.ViewName;
                ExtendModule.TableChange(module,oldModule);
            }
            return new ResultData
            {
                Code = 0,
                Data = dbHelper.Edit(module.Id, module),
                Msg = ""
            };

        }

        // POST: api/GgcmsCategories
        public ResultData Add(GgcmsModule module)
        {
            ResultData result = new ResultData();
            if (!ModelState.IsValid)
            {
                result.Code = 3;
                result.Data = BadRequest(ModelState);
                return result;
            }
            module = dbHelper.Add(module);
            module.TableName = "moduleTab_" + module.Id.ToString();
            module.ViewName = "moduleView_" + module.Id.ToString();
            module = dbHelper.Edit(module.Id, module);
            if (module.Columns!=null)
            {
                ExtendModule.TableCreate(module);
            }
            return result;
        }

        // DELETE: api/GgcmsCategories/5
        public ResultData Delete(int id)
        {

            ExtendModule.TableDelete(id);
            return new ResultData
            {
                Code = 0,
                Msg = "",
                Data = dbHelper.Delete<GgcmsModule>(id)
            };
        }
        [HttpGet]
        public ResultData MultDelete()
        {
            try
            {
                var reqParams = InitRequestParams<GgcmsModule>();
                reqParams.limit = 0;
                reqParams.offset = 0;
                var list = dbHelper.GetList<GgcmsModule>(reqParams);
                foreach (var item in list.List)
                {
                    GgcmsModule module = item as GgcmsModule;
                    ExtendModule.TableDelete(module.Id);
                }
                return new ResultData
                {
                    Code = 0,
                    Msg = "",
                    Data = dbHelper.MultDelete<GgcmsModule>(reqParams)
                };
            } catch (Exception ex)
            {
                return new ResultData
                {
                    Code = 4,
                    Msg = ex.Message,
                    Data = ex
                };
            }
        }


        public ResultData Exists(int id)
        {
            return new ResultData
            {
                Code = 0,
                Msg = "",
                Data = dbHelper.Exists<GgcmsModule>(id)
            };
        }
    }
}