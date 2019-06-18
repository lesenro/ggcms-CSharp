using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GgcmsCSharp.Models
{
    public class ResultInfo
    {
        public int Code { get; set; } = 0;
        public string Msg { get; set; } = "";
        public dynamic Data { get; set; } = null;
    }
}