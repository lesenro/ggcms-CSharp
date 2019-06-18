using GgcmsCSharp.ApiCtrls;
using GgcmsCSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GgcmsCSharp.Utils
{
    public interface IThirdPartyStorage
    {
        string serverUrl { get; set; }
        ResultInfo UploadFile(string file);
        ResultInfo Delete(string key);
    }
}
