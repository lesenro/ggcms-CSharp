using GgcmsCSharp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GgcmsCSharp.Models
{
    public class SearchParams
    {
        public int PageNum { get; set; } = 1;
        public int PageSize { get; set; } = 0;
        public string Columns { get; set; } = "";
        public string QueryString { get; set; } = "";
        public object WhereParams { get; set; } = null;
        public string OrderBy { get; set; } = "";
        public string ToJson(string wrapName = "sParams")
        {
            return Tools.JsonSerialize(this);
        }
    }
}