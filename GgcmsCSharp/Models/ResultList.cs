using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GgcmsCSharp.Models
{
    public class ResultList<T>
    {
        public List<T> List { get; set; }
        public int Count { get; set; }
    }
}