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
            GgcmsModule module = getGgcmsModule(id);
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
        private GgcmsModule getGgcmsModule(int id)
        {
            using (GgcmsDB db = new GgcmsDB())
            {
                GgcmsModule module = db.GgcmsModules.Find(id);
                if (module != null)
                {
                    module.Columns = db.GgcmsModuleColumns.Where(x => x.Module_Id == id).ToList();
                    return module;
                }
                else
                {
                    return null;
                }
            }
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
                GgcmsModule oldModule = getGgcmsModule(module.Id);
                module.TableName = oldModule.TableName;
                module.ViewName = oldModule.ViewName;
                using (GgcmsDB db = new GgcmsDB())
                {
                    int colidx = 1;
                    foreach (var col in module.Columns.FindAll(x=> x.Id>0))
                    {
                        string[] tmp = col.ColName.Split('_');
                        if (tmp.Length == 2)
                        {
                            int idx = int.Parse(tmp[1]);
                            if (idx >= colidx)
                            {
                                colidx = idx+1;
                            }
                        }
                    }
                    foreach (var col in oldModule.Columns.FindAll(x => x.Id > 0))
                    {
                        string[] tmp = col.ColName.Split('_');
                        if (tmp.Length == 2)
                        {
                            int idx = int.Parse(tmp[1]);
                            if (idx >= colidx)
                            {
                                colidx = idx + 1;
                            }
                        }
                        GgcmsModuleColumn newcol = module.Columns.Find(x => x.Id == col.Id);
                        
                        if (newcol == null)
                        {
                            db.GgcmsModuleColumns.Remove(col);
                            db.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[" + module.TableName + "] DROP COLUMN [" + col.ColName + "]");
                        }
                    }
                    foreach (var col in module.Columns)
                    {
                        GgcmsModuleColumn oldcol = oldModule.Columns.Find(x => x.Id == col.Id);
                        if (oldcol == null)
                        {
                            col.Module_Id = module.Id;
                            col.ColName = "col_" + colidx.ToString();
                            db.GgcmsModuleColumns.Add(col);
                            
                            switch (col.ColType)
                            {
                                case "nvarchar":
                                    db.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[" + module.TableName + "] ADD ["+ col.ColName + "] nvarchar("+ (col.Length > 0 ? col.Length.ToString() : "MAX") + ")");
                                    break;
                                case "int":
                                    db.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[" + module.TableName + "] ADD [" + col.ColName + "] int");
                                    break;
                                case "bigint":
                                    db.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[" + module.TableName + "] ADD [" + col.ColName + "] bigint");
                                    break;
                                case "datetime":
                                    db.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[" + module.TableName + "] ADD [" + col.ColName + "] datetime");
                                    break;
                                case "decimal":
                                    db.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[" + module.TableName + "] ADD [" + col.ColName + "] decimal(18" + (col.ColDecimal > 0 && col.ColDecimal < 6 ? "," + col.ColDecimal.ToString() : "") + ")");
                                    break;
                            }
                            colidx++;
                        }
                        else
                        {
                            col.ColName = oldcol.ColName;
                            col.Module_Id = module.Id;
                            var ent=db.Entry(col);
                            ent.State = EntityState.Modified;

                            switch (col.ColType)
                            {
                                case "nvarchar":
                                    db.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[" + module.TableName + "] ALTER COLUMN [" + col.ColName + "] nvarchar(" + (col.Length > 0 ? col.Length.ToString() : "MAX") + ")");
                                    break;
                                case "int":
                                    db.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[" + module.TableName + "] ALTER COLUMN [" + col.ColName + "] int");
                                    break;
                                case "bigint":
                                    db.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[" + module.TableName + "] ALTER COLUMN [" + col.ColName + "] bigint");
                                    break;
                                case "datetime":
                                    db.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[" + module.TableName + "] ALTER COLUMN [" + col.ColName + "] datetime");
                                    break;
                                case "decimal":
                                    db.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[" + module.TableName + "] ALTER COLUMN [" + col.ColName + "] decimal(18" + (col.ColDecimal > 0 && col.ColDecimal < 6 ? "," + col.ColDecimal.ToString() : "") + ")");
                                    break;
                            }
                        }
                        
                        
                    }

                    db.SaveChanges();
                }

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
                using (GgcmsDB db = new GgcmsDB())
                {
                    StringBuilder newTabSql = new StringBuilder();
                    newTabSql.Append("CREATE TABLE [dbo].["+ module.TableName + "] (");
                    newTabSql.Append("[Id] int NOT NULL IDENTITY(1,1) ");
                    newTabSql.Append(",[Articles_Id] int ");
                    int colidx = 1;
                    foreach (var col in module.Columns)
                    {
                        col.Module_Id = module.Id;
                        col.ColName = "col_" + colidx.ToString();
                        db.GgcmsModuleColumns.Add(col);
                        switch(col.ColType)
                        {
                            case "nvarchar":
                                newTabSql.Append(",[" + col.ColName + "] nvarchar(" + (col.Length > 0 ? col.Length.ToString() : "MAX") + ") ");
                                break;
                            case "int":
                                newTabSql.Append(",[" + col.ColName + "] int ");
                                break;
                            case "bigint":
                                newTabSql.Append(",[" + col.ColName + "] bigint ");
                                break;
                            case "datetime":
                                newTabSql.Append(",[" + col.ColName + "] datetime ");
                                break;
                            case "decimal":
                                newTabSql.Append(",[" + col.ColName + "] decimal(18" + (col.ColDecimal > 0 && col.ColDecimal < 6 ? "," + col.ColDecimal.ToString() : "") + ") ");
                                break;
                        }
                        colidx++;
                    }
                    
                    newTabSql.Append(",PRIMARY KEY ([Id]) )");
                    db.Database.ExecuteSqlCommand(newTabSql.ToString(), new object[] { });
                    db.SaveChanges();
                }
                
            }
            return result;
        }

        // DELETE: api/GgcmsCategories/5
        public ResultData Delete(int id)
        {

            using (GgcmsDB db = new GgcmsDB())
            {
                GgcmsModule module = db.GgcmsModules.Find(id);
                if (module != null)
                {
                    db.Database.ExecuteSqlCommand("DROP TABLE [dbo].[" + module.TableName + "]");

                    var collist = db.GgcmsModuleColumns.Where(x => x.Module_Id == module.Id);
                    db.GgcmsModuleColumns.RemoveRange(collist);
                }
            }
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

                var list = dbHelper.GetList<GgcmsModule>(reqParams);
                using (GgcmsDB db = new GgcmsDB())
                {
                    foreach (var item in list.List)
                    {
                        GgcmsModule module = item as GgcmsModule;
                        try
                        {
                            db.Database.ExecuteSqlCommand("DROP TABLE [dbo].[" + module.TableName + "]");
                        }
                        catch
                        {
                        }
                        var collist = db.GgcmsModuleColumns.Where(x => x.Module_Id == module.Id);
                        db.GgcmsModuleColumns.RemoveRange(collist);
                    }
                    db.SaveChanges();
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