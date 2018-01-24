using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace GgcmsCSharp.Utils
{
    public class DataBaseHelper<TP> where TP : DbContext
    {
        public TP dbCxt { get; set; }
        public DataBaseHelper(TP db)
        {
            dbCxt = db;
        }
        public static string DecodeOutputString(string outputstring)
        {
            //要替换的敏感字
            string str_Regex = @"(/sand/s)|(/sand/s)|(/slike/s)|(select/s)|(insert/s)|(delete/s)|(update/s[/s/S].*/sset)|(create/s)|(/stable)|(<[iframe|/iframe|script|/script])|(')|(/sexec)|(/sdeclare)|(/struncate)|(/smaster)|(/sbackup)|(/smid)|(/scount)";
            try
            {
                if ((outputstring != null) && (outputstring != String.Empty))
                {
                    Regex Regex = new Regex(str_Regex, RegexOptions.IgnoreCase);
                    MatchCollection matches = Regex.Matches(outputstring);
                    for (int i = 0; i < matches.Count; i++)
                        outputstring = outputstring.Replace(matches[i].Value, matches[i].Value.Substring(1, matches[i].Value.Length - 2));
                }
            }
            catch
            {
                return "";
            }
            return outputstring;
        }

        public static string OrderInit(RequestParams reqParams)
        {
            reqParams.orderby = "";
            if (!string.IsNullOrEmpty(reqParams.order))
            {
                string[] order = reqParams.order.Split(",".ToCharArray());
                string[] sortby = reqParams.sortby.Split(",".ToCharArray());
                if (order.Length == sortby.Length)
                {
                    List<string> ss = new List<string>();
                    for (int i = 0; i < order.Length; i++)
                    {
                        ss.Add(sortby[i] + " " + order[i]);
                    }
                    reqParams.orderby = string.Join(",", ss);
                }
            }
            return reqParams.orderby;
        }
        public static RequestParams getQuery<T>(RequestParams reqParams) where T : class
        {
            string qs = HttpUtility.UrlDecode(reqParams.query); 
            try
            {
                if (!string.IsNullOrEmpty(qs))
                {
                    List<string> lquery = new List<string>();
                    string[] arr;
                    string condition = " ";
                    if (qs.Contains(";"))
                    {
                        arr = qs.Split(";".ToCharArray());
                        condition = " OR ";
                    }
                    else
                    {
                        arr = qs.Split(",".ToCharArray());
                        condition = " AND ";
                    }
                    int paramIdx = 0;
                    foreach (string s in arr)
                    {
                        string[] qkey = s.Split(":".ToCharArray());
                        if (qkey.Length == 2)
                        {
                            string[] keys = qkey[0].Split(".".ToCharArray());
                            string val = qkey[1];
                            Type t = typeof(T);
                            PropertyInfo pinfo = t.GetProperty(keys[0]);
                            bool isString = pinfo.PropertyType.ToString().ToLower().EndsWith("string");
                            if (isString)
                            {
                                val = "\"" + val + "\"";
                            }
                            if (keys.Length == 1)
                            {
                                lquery.Add(keys[0] + " = " + val);
                            }
                            else if (keys.Length == 2)
                            {
                                string k1 = keys[0];
                                string k2 = keys[1].ToLower();
                                switch (k2)
                                {
                                    case "exact":
                                        lquery.Add(k1 + " = " + val);
                                        break;
                                    case "contains":
                                        lquery.Add(k1 + ".Contains(" + val + ")");
                                        break;
                                    case "gt":
                                        lquery.Add(k1 + " > " + val);
                                        break;
                                    case "gte":
                                        lquery.Add(k1 + " >= " + val);
                                        break;
                                    case "lt":
                                        lquery.Add(k1 + " < " + val);
                                        break;
                                    case "lte":
                                        lquery.Add(k1 + " <= " + val);
                                        break;
                                    case "eq":
                                        lquery.Add(k1 + " = " + val);
                                        break;
                                    case "ne":
                                        lquery.Add(k1 + " <> " + val);
                                        break;
                                    case "startswith":
                                        lquery.Add(k1 + ".StartsWith(" + val + ")");
                                        break;
                                    case "endswith":
                                        lquery.Add(k1 + ".EndsWith(" + val + ")");
                                        break;
                                    case "in":
                                        reqParams.queryParams = new List<object>();
                                        if (isString)
                                        {
                                            //lquery.Add("@" + paramIdx.ToString() + ".Contains(" + k1 + ")");
                                            val = val.Replace("\"", "");
                                            reqParams.queryParams.AddRange(val.Split("|".ToCharArray()));
                                            List<string> strParams = new List<string>();
                                            for (int i = 0; i < reqParams.queryParams.Count; i++)
                                            {
                                                strParams.Add(k1 + ".Contains(@" + paramIdx.ToString() + ")");
                                                paramIdx++;
                                            }
                                            lquery.Add("(" + string.Join(" OR ", strParams) + ")");
                                        }
                                        else
                                        {
                                            lquery.Add("@" + paramIdx.ToString() + ".Contains(outerIt." + k1 + ")");
                                            string[] ids = val.Split("|".ToCharArray());
                                            foreach (string str in ids)
                                            {
                                                reqParams.queryParams.Add(int.Parse(str));
                                            }

                                        }

                                        break;
                                    case "isnull":
                                        if (val == "true")
                                        {
                                            lquery.Add(" ( isnull (" + k1 + ")) ");
                                        }
                                        else
                                        {
                                            lquery.Add(" (not isnull (" + k1 + ")) ");
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    reqParams.query = string.Join(condition, lquery);
                    return reqParams;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            reqParams.query = "";
            return reqParams;
        }
        public ListResult GetList<T>(RequestParams reqParams) where T : class
        {
            IQueryable list;
            int count = 0;
            if (reqParams.queryParams != null && reqParams.queryParams.Count > 0)
            {
                string t = reqParams.queryParams[0].GetType().ToString();
                if (t.ToLower().EndsWith("string"))
                {
                    List<string> ids = new List<string>();
                    foreach (object val in reqParams.queryParams)
                    {
                        ids.Add(val.ToString());
                    }
                    list = dbCxt.Set(typeof(T));
                    list = list.Where(reqParams.query, ids.ToArray());
                    count = dbCxt.Set(typeof(T)).Where(reqParams.query, ids.ToArray()).Count();
                }
                else
                {
                    List<int> ids = new List<int>();
                    foreach (object val in reqParams.queryParams)
                    {
                        ids.Add((int)val);
                    }

                    list = dbCxt.Set(typeof(T));
                    list = list.Where(reqParams.query, ids.ToArray());
                    count = dbCxt.Set(typeof(T)).Where(reqParams.query, ids.ToArray()).Count();
                }
            }
            else
            {
                list = dbCxt.Set(typeof(T));
                if (!string.IsNullOrEmpty(reqParams.query))
                {
                    list = list.Where(reqParams.query);
                    count = dbCxt.Set(typeof(T)).Where(reqParams.query).Count();
                }
                else
                {
                    count = dbCxt.Set(typeof(T)).Count();
                }
            }
            if (!string.IsNullOrEmpty(reqParams.orderby))
            {
                list = list.OrderBy(reqParams.orderby);
            }
            if (!string.IsNullOrEmpty(reqParams.columns))
            {
                list = list.Select(reqParams.columns);
            }
            if (reqParams.limit > 0)
            {
                list = list.Skip(reqParams.offset).Take(reqParams.limit);
            }
            return new ListResult { List = list, Count = count };
        }
        public ListResult GetList<T>(string query="", string sortby = "Id", string order = "desc", int limit=10,int offset=0,string columns="") where T : class
        {
            RequestParams reqParams = RequestParams.GetRequestParams<TP, T>(columns, limit, offset, 1, order, sortby, query);
            return GetList<T>(reqParams);
        }
        public T GetById<T>(long id) where T : class
        {
            var info = dbCxt.Set(typeof(T)).Find(id);

            if (info == null)
            {
                return null;
            }
            else
            {
                return info as T;
            }
        }
        public T Edit<T>(long id, T info) where T : class
        {
            Type t = info.GetType();
            PropertyInfo pinfo = t.GetProperty("Id");

            if (!Exists<T>(id))
            {
                return null;
            }
            try
            {
                var ent = dbCxt.Entry(info);
                ent.State = EntityState.Modified;
                dbCxt.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }
            return info;
        }
        public T Add<T>(T info) where T : class
        {
            dbCxt.Set(typeof(T)).Add(info);
            try
            {
                dbCxt.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return info;
        }
        public T Delete<T>(long id) where T : class
        {
            var info = dbCxt.Set(typeof(T)).Find(id);
            if (info == null)
            {
                return null;
            }

            dbCxt.Set(typeof(T)).Remove(info);
            try
            {
                dbCxt.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return info as T;
        }

        public int MultDelete<T>(RequestParams reqParams) where T : class
        {
            try
            {
                string query = reqParams.query;
                if (reqParams.queryParams.Count > 0)
                {
                    string t = reqParams.queryParams[0].GetType().ToString();
                    IQueryable list;
                    if (t.ToLower().EndsWith("string"))
                    {
                        List<string> ids = new List<string>();
                        foreach (object val in reqParams.queryParams)
                        {
                            ids.Add(val.ToString());
                        }
                        list = dbCxt.Set(typeof(T)).Where(query, ids.ToArray());
                    }
                    else
                    {
                        List<int> ids = new List<int>();
                        foreach (object val in reqParams.queryParams)
                        {
                            ids.Add((int)val);
                        }
                        list = dbCxt.Set(typeof(T)).Where(query, ids.ToArray());
                    }

                    dbCxt.Set(typeof(T)).RemoveRange(list);
                }
                else
                {
                    dbCxt.Set(typeof(T)).RemoveRange(dbCxt.Set(typeof(T)).Where(query));
                }

                return dbCxt.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Exists<T>(long id)
        {
            return dbCxt.Set(typeof(T)).Where("Id = " + id.ToString()).Count() > 0;
        }
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbCxt.Dispose();
            }
        }
    }
    public class RequestParams
    {
        public string columns { get; set; } = "";
        public int limit { get; set; } = 10;
        public int offset { get; set; } = 0;
        public int pagenum { get; set; } = 1;
        public string order { get; set; } = "";
        public string orderby { get; set; } = "";
        public string sortby { get; set; } = "";
        public string query { get; set; } = "";
        public List<object> queryParams { get; set; } = new List<object>();
        public RequestParams()
            : base()
        {

        }
        public static RequestParams GetRequestParams<DC, DT>(string columns, int limit, int offset, int pagenum, string order, string sortby, string query) where DC : DbContext where DT : class
        {
            var reqParams = new RequestParams();
            reqParams.columns = DataBaseHelper<DC>.DecodeOutputString(columns);
            if (!string.IsNullOrEmpty(columns))
            {
                reqParams.columns = "New(" + columns + ")";
            }
            reqParams.limit = limit;
            reqParams.offset = offset;
            reqParams.pagenum = pagenum;

            reqParams.query = DataBaseHelper<DC>.DecodeOutputString(query);
            reqParams = DataBaseHelper<DC>.getQuery<DT>(reqParams);

            reqParams.order = DataBaseHelper<DC>.DecodeOutputString(order);
            reqParams.sortby = DataBaseHelper<DC>.DecodeOutputString(sortby);
            reqParams.orderby = DataBaseHelper<DC>.OrderInit(reqParams);
            return reqParams;
        }

    }
    public class ListResult
    {
        public IQueryable List { get; set; }
        public int Count { get; set; }
        public List<dynamic> ToList()
        {
            List<dynamic> list = new List<dynamic>();
            foreach(var item in List)
            {
                list.Add(item);
            }
            return list;
        }
    }
}
