using GgcmsCSharp.Models;
using System.Collections.Generic;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Linq;
using System;
using System.Data.Entity;
using System.Web;
using GgcmsCSharp.Utils;

namespace GgcmsCSharp.ApiCtrls
{
    public class GgcmsModulesController : ApiBaseController
    {

        // GET: api/GgcmsCategories
        [HttpGet]
        public IHttpActionResult GetList(string query)
        {
            string json = HttpUtility.UrlDecode(query);
            SearchParams sParams = Tools.JsonDeserialize<SearchParams>(json);

            return Ok(GetRecords<GgcmsModules>(sParams));
        }

        // GET: api/GgcmsCategories/5
        public IHttpActionResult GetInfo(int id)
        {

            GgcmsModules module = ExtendModule.GetGgcmsModule(id);
            if (module != null)
            {
                return Ok(module);
            }

            return BadRequest("信息不存在");

        }
        
        // PUT: api/GgcmsCategories/5
        public IHttpActionResult Edit(GgcmsModules module)
        {

            if (module.Columns != null)
            {
                GgcmsModules oldModule = ExtendModule.GetGgcmsModule(module.Id);
                module.TableName = oldModule.TableName;
                module.ViewName = oldModule.ViewName;
                ExtendModule.TableChange(module, oldModule);
            }
            Dbctx.SaveChanges();
            ClearCache();
            return Ok(module);
        }

        // POST: api/GgcmsCategories
        public IHttpActionResult Add(GgcmsModules module)
        {
            var result = Dbctx.GgcmsModules.Add(module);
            Dbctx.SaveChanges();
            result.TableName = "moduleTab_" + result.Id.ToString();
            result.ViewName = "moduleView_" + result.Id.ToString();
            ExtendModule.TableCreate(module);
            Dbctx.SaveChanges();
            ClearCache();
            return Ok(result);

        }

        // DELETE: api/GgcmsCategories/5
        [HttpGet]
        public IHttpActionResult Delete(int id)
        {

            ExtendModule.TableDelete(id);
            GgcmsModules module = Dbctx.GgcmsModules.Where(x => x.Id == id).FirstOrDefault();
            if (module != null)
            {
                Dbctx.GgcmsModules.Remove(module);
                Dbctx.SaveChanges();
            }
            ClearCache();
            return Ok(id);

        }
        [HttpPost]
        public IHttpActionResult MultDelete(int[] ids)
        {
            try
            {
                var query = Dbctx.GgcmsModules.Where(x => ids.Contains(x.Id));
                foreach (var item in query.ToList())
                {
                    GgcmsModules module = item as GgcmsModules;
                    ExtendModule.TableDelete(module.Id);
                }
                Dbctx.GgcmsModules.RemoveRange(query);
                int c = Dbctx.SaveChanges();
                ClearCache();
                return Ok(c);

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        public IHttpActionResult Exists(int id)
        {
            return Ok(Dbctx.GgcmsModules.Where(x => x.Id == id).Count() > 0);
        }
    }
}