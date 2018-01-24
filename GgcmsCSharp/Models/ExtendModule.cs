using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace GgcmsCSharp.Models
{
    public class ExtendModule
    {
        public static GgcmsModule GetModuleData(int aid, int mid)
        {
            var module = GetGgcmsModule(mid);
            using (GgcmsDB db = new GgcmsDB())
            {
                string sql = "SELECT * FROM [" + module.TableName + "] WHERE [Articles_Id] = @aid";
                var mdata = db.Database.SqlQuery<Dictionary<string,dynamic>>(sql, new SqlParameter("@aid", aid)).First();
                foreach (var col in module.Columns)
                {
                    
                }
            }
            return module;
        }
        public static GgcmsModule GetGgcmsModule(int id)
        {
            using (GgcmsDB db = new GgcmsDB())
            {
                GgcmsModule module = db.GgcmsModules.Find(id);
                if (module != null)
                {
                    module.Columns = db.GgcmsModuleColumns.Where(x => x.Module_Id == id).ToList();
                }
                return module;
            }
        }
        public static Dictionary<string, dynamic> SaveData(int aid , GgcmsModule module)
        {
            using (GgcmsDB db = new GgcmsDB())
            {
                var cols = db.GgcmsModuleColumns.Where(x => x.Module_Id == module.Id);
                var m = db.GgcmsModules.Find(module.Id);
                Dictionary<string, dynamic> result = new Dictionary<string, dynamic>();
                foreach (var col in cols)
                {
                    var c = module.Columns.Find(x => x.Id == col.Id);
                    if (c != null)
                    {
                        col.Value = c.Value;
                        result.Add(col.ColTitle, c.Value);
                    }
                }

                string sql = "SELECT COUNT(*) FROM [" + m.TableName + "] WHERE Articles_Id= @aid";

                //添加
                if (db.Database.SqlQuery<int>(sql, new SqlParameter("@aid", aid)).First() == 0)
                {
                    List<string> colsStr = new List<string>();
                    List<string> paramStr = new List<string>();
                    colsStr.Add("[Articles_Id]");
                    paramStr.Add("Articles_Id");

                    List<SqlParameter> sqlParams = new List<SqlParameter>();
                    foreach (var col in cols)
                    {
                        colsStr.Add("[" + col.ColName + "]");
                        paramStr.Add("@" + col.ColName);
                        sqlParams.Add(new SqlParameter("@" + col.ColName, col.Value));
                    }
                    sql = "INSERT INTO [" + m.TableName + "] ( " + string.Join(",", colsStr) + " ) VALUES (  " + string.Join(",", paramStr) + " )";
                    db.Database.ExecuteSqlCommand(sql, sqlParams);
                }
                //修改
                else
                {
                    List<string> colsStr = new List<string>();
                    List<SqlParameter> sqlParams = new List<SqlParameter>();

                    foreach (var col in cols)
                    {
                        colsStr.Add("[" + col.ColName + "] = @" + col.ColName);
                        sqlParams.Add(new SqlParameter("@" + col.ColName, col.Value));
                    }

                    sqlParams.Add(new SqlParameter("@Articles_Id", aid));

                    sql = "UPDATE [" + m.TableName + "] SET " + string.Join(",", colsStr) + " Where [Articles_Id] = @Articles_Id";
                    db.Database.ExecuteSqlCommand(sql, sqlParams.ToArray());
                    
                }
                return result;
            }
        }
        public static int Delete(int aid , int mid)
        {
            using (GgcmsDB db = new GgcmsDB())
            {
                var module = db.GgcmsModules.Find(mid);
                string sql = "DELETE FROM [" + module.TableName + "] Where [Articles_Id] = @Articles_Id";
                System.Data.DataTable dt = new System.Data.DataTable();
                return db.Database.ExecuteSqlCommand(sql, new SqlParameter("@Articles_Id", aid));
                
            }
            
        }
        public static void TableChange(GgcmsModule module, GgcmsModule oldModule)
        {
            using (GgcmsDB db = new GgcmsDB())
            {
                int colidx = 1;
                foreach (var col in module.Columns.FindAll(x => x.Id > 0))
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
                        var c = db.GgcmsModuleColumns.Where(x => x.Id == col.Id).First();
                        db.GgcmsModuleColumns.Remove(c);
                        db.Database.ExecuteSqlCommand("ALTER TABLE [" + module.TableName + "] DROP COLUMN [" + col.ColName + "]");
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
                                db.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[" + module.TableName + "] ADD [" + col.ColName + "] nvarchar(" + (col.Length > 0 ? col.Length.ToString() : "MAX") + ")");
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
                        var ent = db.Entry(col);
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
        public static void TableCreate(GgcmsModule module)
        {
            using (GgcmsDB db = new GgcmsDB())
            {
                StringBuilder newTabSql = new StringBuilder();
                newTabSql.Append("CREATE TABLE [dbo].[" + module.TableName + "] (");
                newTabSql.Append("[Id] int NOT NULL IDENTITY(1,1) ");
                newTabSql.Append(",[Articles_Id] int ");
                int colidx = 1;
                foreach (var col in module.Columns)
                {
                    col.Module_Id = module.Id;
                    col.ColName = "col_" + colidx.ToString();
                    db.GgcmsModuleColumns.Add(col);
                    switch (col.ColType)
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
        public static void TableDelete(int id)
        {
            using (GgcmsDB db = new GgcmsDB())
            {
                GgcmsModule module = db.GgcmsModules.Find(id);
                if (module != null)
                {
                    TableDelete(module);
                }
            }
        }
        public static void TableDelete(GgcmsModule module)
        {
            using (GgcmsDB db = new GgcmsDB())
            {
                db.Database.ExecuteSqlCommand("DROP TABLE [" + module.TableName + "]");

                var collist = db.GgcmsModuleColumns.Where(x => x.Module_Id == module.Id);
                db.GgcmsModuleColumns.RemoveRange(collist);
                db.SaveChanges();
            }
        }
    }
}