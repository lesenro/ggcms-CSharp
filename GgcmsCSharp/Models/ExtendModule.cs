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
        public static GgcmsModule GetModuleData(int aid, int mid)
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
        public static Dictionary<string, dynamic> GetModuleToDict(int aid , GgcmsModule module){
            
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
                            result.Add(col.ColName, reader[col.ColName]);
                        }
                    }
                    reader.Close();
                }
            }
            return result;
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
        public static DataTable ViewArticles(int mid, RequestParams reqParams)
        {
            var module = GetGgcmsModule(mid);
            StringBuilder sql = new StringBuilder();
            if (reqParams.pagenum == 1)
            {
                sql.Append("select ");
                if (reqParams.limit > 0)
                {
                    sql.Append("TOP " + reqParams.limit.ToString() + " ");
                }
                sql.Append(string.IsNullOrWhiteSpace(reqParams.columns) ? "* " : reqParams.columns);
                sql.Append(" From " + module.ViewName);
                if (!string.IsNullOrWhiteSpace(reqParams.query))
                {
                    sql.Append(" WHERE " + reqParams.query);
                }
                if (!string.IsNullOrWhiteSpace(reqParams.orderby))
                {
                    sql.Append(" ORDER BY " + reqParams.orderby);
                }
            }
            else
            {
                reqParams.orderby = string.IsNullOrWhiteSpace(reqParams.orderby) ? "Id DESC" : reqParams.orderby;
                reqParams.query = string.IsNullOrWhiteSpace(reqParams.query) ? "" :" WHERE" +reqParams.orderby;
                reqParams.columns = string.IsNullOrWhiteSpace(reqParams.columns) ? "*" : reqParams.columns;
                reqParams.offset++;
                reqParams.limit = reqParams.offset + reqParams.limit;
                //select t1.id,Content,Title FROM GgcmsArticles t1 INNER JOIN (select t2.Id  from(select row_number()  over (order by id DESC)r_num,Id FROM GgcmsArticles WHERE Id>23) t2 WHERE t2.r_num BETWEEN 2 and 4)t3 on t1.id=t3.id
                sql.Append("select t1.Id, "+reqParams.columns + " FROM "+ module.ViewName+ " t1 INNER JOIN ");
                sql.Append("(select t2.Id  from(select row_number()  over (order by " + reqParams.orderby + ")r_num,Id FROM " + module.ViewName + " " + reqParams.query + ") t2 WHERE t2.r_num BETWEEN " + reqParams.offset + " and " + reqParams.limit + ") t3 on t1.Id=t3.Id");
            }
            DataTable dt = new DataTable(module.ViewName);
            using (GgcmsDB db = new GgcmsDB())
            {
                SqlDataAdapter da = new SqlDataAdapter(sql.ToString(), db.Database.Connection as SqlConnection);
                da.Fill(dt);
            }
            if (dt.Columns.Contains("RedirectUrl"))
            {
                string Prefix = ConfigurationManager.AppSettings["UploadPrefix"].ToString();
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
            ViewCreate(module);
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
            ViewCreate(module);
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
            ViewDelete(module);
            using (GgcmsDB db = new GgcmsDB())
            {
                db.Database.ExecuteSqlCommand("DROP TABLE [" + module.TableName + "]");

                var collist = db.GgcmsModuleColumns.Where(x => x.Module_Id == module.Id);
                db.GgcmsModuleColumns.RemoveRange(collist);
                db.SaveChanges();
            }
        }
        private static void ViewCreate(GgcmsModule module)
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
        private static void ViewDelete(GgcmsModule module)
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