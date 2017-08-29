using GgcmsCSharp.Models;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Linq.Dynamic;

using System.Net.Http;
using System.Web;
using System.Collections.Generic;
using System.Reflection;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace GgcmsCSharp.ApiCtrls
{
    public class RequestParams
    {
        public string columns { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public int pagenum { get; set; }
        public string order { get; set; }
        public string orderby { get; set; }
        public string sortby { get; set; }
        public string query { get; set; }
        public List<object> queryParams { get; set; }
    }
    public class dbTools<T> where T : class
    {
        public GgcmsDB db = new GgcmsDB();
        private HttpRequestMessage Request;
        public RequestParams reqParams { get; set; }
        public string DecodeOutputString(string outputstring)
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
        private void initRequestParams()
        {
            reqParams.columns = DecodeOutputString(Request.GetQueryString("columns")??"");
            if (!string.IsNullOrEmpty(reqParams.columns))
            {
                reqParams.columns = "New(" + reqParams.columns + ")";
            }
            reqParams.limit = int.Parse(Request.GetQueryString("limit")??"10");
            reqParams.offset = int.Parse(Request.GetQueryString("offset")??"0");
            reqParams.pagenum = int.Parse(Request.GetQueryString("pagenum")??"1");

            reqParams.query = DecodeOutputString(Request.GetQueryString("query"));
            reqParams.query = getQuery(reqParams.query);

            reqParams.order = DecodeOutputString(Request.GetQueryString("order") ?? "");
            reqParams.sortby = DecodeOutputString(Request.GetQueryString("sortby") ?? "");
            orderInit();
        }
        public void orderInit()
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
        }
        public string getQuery(string qs)
        {
            try
            {
                if (!string.IsNullOrEmpty(qs))
                {
                    List<string> lquery = new List<string>();
                    string[] arr = qs.Split(",".ToCharArray());
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
                                val = "'" + val + "'";
                            }
                            if (keys.Length == 1)
                            {
                                lquery.Add(keys[0] + " = " + val);
                            }
                            else if (keys.Length == 2)
                            {
                                string k1 = keys[0].ToLower();
                                string k2 = keys[1].ToLower();
                                switch (k2)
                                {
                                    case "exact":
                                        lquery.Add(k1 + " = " + val);
                                        break;
                                    case "contains":
                                        lquery.Add(k1 + " like " + "'%" + val.Replace("'", "") + "%'");
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
                                        lquery.Add(k1 + " like " + "'" + val.Replace("'", "") + "%'");
                                        break;
                                    case "endswith":
                                        lquery.Add(k1 + " like " + "'%" + val.Replace("'", "") + "'");
                                        break;
                                    case "in":
                                        lquery.Add("@" + paramIdx.ToString() + ".Contains(outerIt." + k1 + ")");
                                        if (isString)
                                        {
                                            reqParams.queryParams.AddRange(val.Split("|".ToCharArray()));
                                        }
                                        else
                                        {
                                            string[] ids = val.Split("|".ToCharArray());
                                            foreach(string str in ids)
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
                    return string.Join(" And ", lquery);
                }
            }
            catch
            {
                return "";
            }
            return "";
        }
        public dbTools(HttpRequestMessage req)
        {
            this.Request = req;
            reqParams = new RequestParams();
            reqParams.queryParams = new List<object>();
        }
        public dbTools(RequestParams rparams)
        {
            reqParams = rparams;
            if (!string.IsNullOrEmpty(reqParams.columns))
            {
                reqParams.columns = "New(" + reqParams.columns + ")";
            }
            reqParams.queryParams = new List<object>();
            reqParams.query= getQuery(reqParams.query);
            orderInit();
        }
        public ResultData GetList(bool initparam=true)
        {
            if (initparam)
            {
                initRequestParams();
            }
            IQueryable list;
            int count = 0;
            if (reqParams.queryParams.Count > 0)
            {
                string t = reqParams.queryParams[0].GetType().ToString();
                if (t.ToLower().EndsWith("string"))
                {
                    List<string> ids = new List<string>();
                    foreach (object val in reqParams.queryParams)
                    {
                        ids.Add(val.ToString());
                    }
                    list = db.Set(typeof(T)).Where(reqParams.query, ids.ToArray())
                    .OrderBy(reqParams.orderby)
                    .Skip(reqParams.offset)
                    .Take(reqParams.limit)
                    .Select(reqParams.columns);
                    count = db.Set(typeof(T)).Where(reqParams.query, ids.ToArray()).Count();
                }
                else
                {
                    List<int> ids = new List<int>();
                    foreach (object val in reqParams.queryParams)
                    {
                        ids.Add((int)val);
                    }
                    list = db.Set(typeof(T)).Where(reqParams.query, ids.ToArray())
                     .OrderBy(reqParams.orderby)
                    .Skip(reqParams.offset)
                    .Take(reqParams.limit)
                    .Select(reqParams.columns);
                    count = db.Set(typeof(T)).Where(reqParams.query, ids.ToArray()).Count();
                }
            }
            else
            {
                list = db.Set(typeof(T))
                   .Where(reqParams.query)
                   .OrderBy(reqParams.orderby)
                   .Skip(reqParams.offset)
                   .Take(reqParams.limit)
                   .Select(reqParams.columns);
                count = db.Set(typeof(T)).Where(reqParams.query).Count();
            }
            ResultData result = new ResultData
            {
                Code = 0,
                Msg = "",
                Data = new { List = list, Count = count }
            };
            return result;
        }
        public IQueryable GetInfoList()
        {
            string query = Request.GetQueryString("query");
            query = DecodeOutputString(query);
            query = getQuery(query);
            string t = reqParams.queryParams[0].GetType().ToString();
            List<int> ids = new List<int>();
            foreach (object val in reqParams.queryParams)
            {
                ids.Add((int)val);
            }
            IQueryable list = db.Set(typeof(T)).Where(query, ids.ToArray());
            return list;
        }
        public ResultData GetById(int id)
        {
            var info = db.Set(typeof(T)).Find(id);
            ResultData result = new ResultData
            {
                Code = 0,
                Msg = ""
            };
            if (info == null)
            {
                result.Code = 1;
                result.Msg = "not found";
            }
            else
            {
                result.Data = info;
            }

            return result;

        }
        public ResultData Edit(int id, T info) 
        {
            Type t = info.GetType();
            PropertyInfo pinfo = t.GetProperty("Id");
            ResultData result = new ResultData
            {
                Code = 0,
                Msg = ""
            };
            if (!(bool)Exists(id).Data)
            {
                result.Code = 1;
                result.Msg = "not found";
                return result;
            }
            try
            {
                var ent = db.Entry(info);
                ent.State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                result.Code = 2;
                result.Data = ex;
                result.Msg = ex.Message;
            }

            return result;
        }
        public ResultData Add(T info)
        {
            ResultData result = new ResultData
            {
                Code = 0,
                Msg = ""
            };
            db.Set(typeof(T)).Add(info);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                result.Code = 2;
                result.Data = ex;
                result.Msg = ex.Message;
            }
            return result;
        }
        public ResultData Delete(int id)
        {
            ResultData result = new ResultData
            {
                Code = 0,
                Msg = "sucess"
            };
            var info = db.Set(typeof(T)).Find(id);
            if (info == null)
            {
                result.Code = 1;
                result.Msg = "not found";
                return result;
            }

            db.Set(typeof(T)).Remove(info);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                result.Code = 2;
                result.Data = ex;
                result.Msg = ex.Message;
            }
            return result;
        }
        public ResultData MultDelete()
        {
            return MultDelete(Request.GetQueryString("query"));
        }
        public ResultData MultDelete(string query)
        {
            try
            {
                query = DecodeOutputString(query);
                query = getQuery(query);
                
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
                        list = db.Set(typeof(T)).Where(query, ids.ToArray());
                    }
                    else
                    {
                        List<int> ids = new List<int>();
                        foreach (object val in reqParams.queryParams)
                        {
                            ids.Add((int)val);
                        }
                        list = db.Set(typeof(T)).Where(query, ids.ToArray());
                    }

                    db.Set(typeof(T)).RemoveRange(list);
                }
                else
                {
                    db.Set(typeof(T)).RemoveRange(db.Set(typeof(T)).Where(query));
                }

                int d = db.SaveChanges();
                return new ResultData
                {
                    Code = 0,
                    Msg = "",
                    Data = d,
                };
            }
            catch (Exception ex)
            {
                return new ResultData
                {
                    Code = 1,
                    Msg = ex.Message,
                    Data = ex,
                };
            }
        }
        public ResultData Exists(int id)
        {

            int count = db.Set(typeof(T)).Where("Id = " + id.ToString()).Count();
            ResultData result = new ResultData
            {
                Code = 0,
                Msg = "sucess",
                Data = count > 0
            };
            return result;
        }
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
        }
    }
}