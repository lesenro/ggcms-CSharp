using GgcmsCSharp.ApiCtrls;
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
        ResultData UploadFile(string file);
        ResultData Delete(string key);
    }
}
