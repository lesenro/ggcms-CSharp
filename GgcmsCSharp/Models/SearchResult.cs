using GgcmsCSharp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GgcmsCSharp.Models
{
    public class SearchResult<T>
    {
        public long Count { get; set; }
        public List<dynamic> Records { get; set; }
        public List<T> GetList()
        {
            List<T> result = new List<T>();
            if (Records?.Count() > 0)
            {
                foreach (dynamic item in Records)
                {
                    string json = Tools.JsonSerialize(item);
                    T obj = Tools.JsonDeserialize<T>(json);
                    result.Add(obj);
                }
                //foreach (dynamic item in Records)
                //{

                //    var ps = ot.GetProperties();
                //    T obj = Activator.CreateInstance<T>();
                //    foreach (var op in ps)
                //    {
                //        if (item[op.Name] == null)
                //        {
                //            continue;
                //        }
                //        var attrs = op.GetCustomAttributes(true);
                //        if (attrs.Where(x => x is System.Web.Script.Serialization.JsonIgnoreAttribute).Count() > 0)
                //        {
                //            continue;
                //        }
                //        try
                //        {
                //            op.SetValue(obj, item[op.Name], null);
                //        }
                //        catch (Exception ex)
                //        {
                //            string fname = op.PropertyType.FullName;
                //            if (fname.Contains("System.DateTime"))
                //            {
                //                op.SetValue(obj, Convert.ToDateTime(item[op.Name]), null);
                //            }
                //        }
                //    }

                //    result.Add(obj);
                //}
            }
            return result;
        }
    }
    }