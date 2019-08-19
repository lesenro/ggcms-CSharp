using GgcmsCSharp.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        public static GgcmsModules GetModuleData(int aid, int mid)
        {
            var module = GetGgcmsModule(mid);
            using (GgcmsDB db = new GgcmsDB())
            {
                using (db.Database.Connection)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("SELECT [Id] ");
                    foreach (var col in module.Columns)
                    {
                        sql.Append(",[" + col.ColName + "] ");
                    }
                    sql.Append("FROM [" + module.TableName + "] WHERE [Articles_Id] = @aid");

                    SqlCommand command = new SqlCommand(sql.ToString(), db.Database.Connection as SqlConnection);
                    command.Parameters.Add(new SqlParameter("@aid", aid));
                    db.Database.Connection.Open();

                    SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                    if (reader.HasRows && reader.Read())
                    {
                        foreach (var col in module.Columns)
                        {
                            col.Value = reader[col.ColName];
                        }
                    }
                    reader.Close();
                }
            }
            return module;
        }
        public static Dictionary<string, dynamic> GetModuleToDict(int aid, int mid)
        {
            var module = GetGgcmsModule(mid);
            if (module == null)
            {
                return null;
            }
            return GetModuleToDict(aid, module);
        }
        public static Dictionary<string, dynamic> GetModuleToDict(int aid , GgcmsModules module,bool isColName=true){
            
            Dictionary<string, dynamic> result = new Dictionary<string, dynamic>();
            result.Add("Id", aid);
            result.Add("Mid", module.Id);
            using (GgcmsDB db = new GgcmsDB())
            {
                using (db.Database.Connection)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("SELECT [Id] ");
                    foreach (var col in module.Columns)
                    {
                        sql.Append(",[" + col.ColName + "] ");
                    }
                    sql.Append("FROM [" + module.TableName + "] WHERE [Articles_Id] = @aid");

                    SqlCommand command = new SqlCommand(sql.ToString(), db.Database.Connection as SqlConnection);
                    command.Parameters.Add(new SqlParameter("@aid", aid));
                    db.Database.Connection.Open();

                    SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                    if (reader.HasRows && reader.Read())
                    {
                        foreach (var col in module.Columns)
                        {
                            if (isColName)
                            {
                                result.Add(col.ColName, reader[col.ColName]);
                            }
                            else
                            {
                                result.Add(col.ColKey, reader[col.ColName]);
                            }
                        }
                    }
                    reader.Close();
                }
            }
            return result;
        }
        public static GgcmsModules GetGgcmsModule(int id)
        {
            using (GgcmsDB db = new GgcmsDB())
            {
                GgcmsModules module = db.GgcmsModules.Find(id);
                if (module != null)
                {
                    module.Columns = db.GgcmsModuleColumns.Where(x => x.Module_Id == id).ToList();
                }
                return module;
            }
        }
        public static Dictionary<string, dynamic> SaveData(int aid , GgcmsModules module)
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
                    paramStr.Add(aid.ToString());

                    List<SqlParameter> sqlParams = new List<SqlParameter>();
                    foreach (var col in cols)
                    {
                        colsStr.Add("[" + col.ColName + "]");
                        paramStr.Add("@" + col.ColName);
                        sqlParams.Add(new SqlParameter("@" + col.ColName, col.Value??""));
                    }
                    sql = "INSERT INTO [" + m.TableName + "] ( " + string.Join(",", colsStr) + " ) VALUES (  " + string.Join(",", paramStr) + " )";
                    db.Database.ExecuteSqlCommand(sql, sqlParams.ToArray());
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
        public static DataTable ViewArticles(int mid, SearchParams reqParams)
        {
            var module = GetGgcmsModule(mid);
            StringBuilder sql = new StringBuilder();
            if (reqParams.PageNum == 1)
            {
                sql.Append("select ");
                if (reqParams.PageSize > 0)
                {
                    sql.Append("TOP " + reqParams.PageSize.ToString() + " ");
                }
                sql.Append(string.IsNullOrWhiteSpace(reqParams.Columns) ? "* " : reqParams.Columns);
                sql.Append(" From " + module.ViewName);
                if (!string.IsNullOrWhiteSpace(reqParams.QueryString))
                {
                    sql.Append(" WHERE " + reqParams.QueryString);
                }
                if (!string.IsNullOrWhiteSpace(reqParams.OrderBy))
                {
                    sql.Append(" ORDER BY " + reqParams.OrderBy);
                }
            }
            else
            {
                reqParams.OrderBy = string.IsNullOrWhiteSpace(reqParams.OrderBy) ? "Id DESC" : reqParams.OrderBy;
                reqParams.QueryString = string.IsNullOrWhiteSpace(reqParams.QueryString) ? "" :" WHERE " +reqParams.QueryString;
                reqParams.Columns = string.IsNullOrWhiteSpace(reqParams.Columns) ? "*" : reqParams.Columns;
                int limit = reqParams.PageSize;
                int offset = (reqParams.PageNum - 1) * reqParams.PageSize;
                //select t1.id,Content,Title FROM GgcmsArticles t1 INNER JOIN (select t2.Id  from(select row_number()  over (order by id DESC)r_num,Id FROM GgcmsArticles WHERE Id>23) t2 WHERE t2.r_num BETWEEN 2 and 4)t3 on t1.id=t3.id
                sql.Append("select t1.Id, "+reqParams.Columns + " FROM "+ module.ViewName+ " t1 INNER JOIN ");
                sql.Append("(select t2.Id  from(select row_number()  over (order by " + reqParams.OrderBy + ")r_num,Id FROM " + module.ViewName + " " + reqParams.QueryString + ") t2 WHERE t2.r_num BETWEEN " + offset + " and " + limit + ") t3 on t1.Id=t3.Id");
            }
            DataTable dt = new DataTable(module.ViewName);
            using (GgcmsDB db = new GgcmsDB())
            {
                SqlDataAdapter da = new SqlDataAdapter(sql.ToString(), db.Database.Connection as SqlConnection);
                da.Fill(dt);
            }
            if (dt.Columns.Contains("RedirectUrl"))
            {
                string Prefix = ConfigurationManager.AppSettings["LinkPrefix"].ToString();
                foreach (DataRow row in dt.Rows)
                {
                    
                    string rurl = row["RedirectUrl"].ToString().Trim();
                    if (string.IsNullOrEmpty(rurl))
                    {
                        row.BeginEdit();
                        row["RedirectUrl"] = Prefix + "/Article/" + row["Id"].ToString();
                        row.EndEdit();
                    }
                }
                dt.AcceptChanges();
            }
            return dt;
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
        /// <summary>
        /// 判断是否有重复的colkey
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public static bool ColumnsCheck(GgcmsModules module)
        {
            var cols = from c in module.Columns
                       group c by c.ColKey
                       into g
                       select  g.Count();
            return cols.Where(x => x > 1).Count() == 0;
        }
        public static void TableChange(GgcmsModules module, GgcmsModules oldModule)
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
                    GgcmsModuleColumns newcol = module.Columns.Find(x => x.Id == col.Id);

                    if (newcol == null)
                    {
                        var c = db.GgcmsModuleColumns.Where(x => x.Id == col.Id).First();
                        db.GgcmsModuleColumns.Remove(c);
                        db.Database.ExecuteSqlCommand("ALTER TABLE [" + module.TableName + "] DROP COLUMN [" + col.ColName + "]");
                    }
                }
                foreach (var col in module.Columns)
                {
                    GgcmsModuleColumns oldcol = oldModule.Columns.Find(x => x.Id == col.Id);
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
                            case "ntext":
                                db.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[" + module.TableName + "] ADD [" + col.ColName + "] ntext");
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
                            case "ntext":
                                db.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[" + module.TableName + "] ALTER COLUMN [" + col.ColName + "] ntext");
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
            ViewCreate(module);
        }
        public static void TableCreate(GgcmsModules module)
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
                        case "ntext":
                            newTabSql.Append(",[" + col.ColName + "] ntext ");
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
                        default:
                            newTabSql.Append(",[" + col.ColName + "] " + col.ColType + " ");
                            break;
                    }
                    colidx++;
                }

                newTabSql.Append(",PRIMARY KEY ([Id]) )");
                db.Database.ExecuteSqlCommand(newTabSql.ToString(), new object[] { });
                db.SaveChanges();
            }
            ViewCreate(module);
        }
        public static void TableDelete(int id)
        {
            using (GgcmsDB db = new GgcmsDB())
            {
                GgcmsModules module = db.GgcmsModules.Find(id);
                if (module != null)
                {
                    TableDelete(module);
                }
            }
        }
        public static void TableDelete(GgcmsModules module)
        {
            ViewDelete(module);
            using (GgcmsDB db = new GgcmsDB())
            {
                db.Database.ExecuteSqlCommand("DROP TABLE [" + module.TableName + "]");

                var collist = db.GgcmsModuleColumns.Where(x => x.Module_Id == module.Id);
                db.GgcmsModuleColumns.RemoveRange(collist);
                db.SaveChanges();
            }
        }
        private static void ViewCreate(GgcmsModules module)
        {
            string[] articleField = "Title,Hits,CreateTime,TitleImg,RedirectUrl,ExtModelId,ShowType,ShowLevel,Category_Id".Split(",".ToArray());
            StringBuilder sql = new StringBuilder();
            sql.Append("CREATE VIEW [" + module.ViewName + "] AS ");
            sql.Append("SELECT GgcmsArticles.Id ");
            foreach (string col in articleField)
            {
                sql.Append(",GgcmsArticles." + col + " ");
            }
            foreach (var col in module.Columns)
            {
                sql.Append("," + module.TableName + "." + col.ColName + " ");
            }
            sql.Append("FROM GgcmsArticles INNER JOIN " + module.TableName + " ON GgcmsArticles.Id = " + module.TableName + ".Articles_Id");
            //dbo.moduleTab_7.col_2,
            ViewDelete(module);
            using (GgcmsDB db = new GgcmsDB())
            {
                db.Database.ExecuteSqlCommand(sql.ToString());
                db.SaveChanges();
            }
            
        }
        private static void ViewDelete(GgcmsModules module)
        {
            try
            {
                string sql = "DROP VIEW [" + module.ViewName + "]";
                using (GgcmsDB db = new GgcmsDB())
                {
                    db.Database.ExecuteSqlCommand(sql);
                    db.SaveChanges();
                }
            }
            catch { }
        }
    }
}