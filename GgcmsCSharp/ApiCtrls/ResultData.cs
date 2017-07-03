using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GgcmsCSharp.ApiCtrls
{
    public class ResultData
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public dynamic Data { get; set; }
    }
}