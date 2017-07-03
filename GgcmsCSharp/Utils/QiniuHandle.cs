using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GgcmsCSharp.ApiCtrls;
using Qiniu.IO;
using Qiniu.Util;
using Qiniu.IO.Model;
using Qiniu.Http;
using System.IO;
using Qiniu.RS;

namespace GgcmsCSharp.Utils
{
    public class QiniuHandle : IThirdPartyStorage
    {
        public string serverUrl { get; set; }

        private string AccessKey;
        private string SecretKey;
        private string bucket;
        private FormUploader uploader;
        private Mac mac;
        PutPolicy putPolicy;
        public QiniuHandle(string svrUrl)
        {
            Dictionary<string, string> cfgs = CacheHelper.GetSysConfigs();

            this.serverUrl = svrUrl;
            this.AccessKey = cfgs["cfg_access_key"];
            this.SecretKey = cfgs["cfg_secret_key"];
            this.bucket = cfgs["cfg_bucket"];
            uploader = new FormUploader();
            mac = new Mac(AccessKey, SecretKey);
            putPolicy = new PutPolicy();
            putPolicy.Scope = bucket;
            putPolicy.SetExpires(3600);

        }
        public ResultData Delete(string key)
        {
            BucketManager bm = new BucketManager(mac);

            HttpResult result = bm.Delete(bucket, key);

            return new ResultData
            {
                Code = result.Code == 200 ? 0 : result.Code,
                Msg = result.RefText,
                Data = result,
            };
        }

        public ResultData UploadFile(string file)
        {
            string jstr = putPolicy.ToJsonString();
            string token = Auth.CreateUploadToken(mac, jstr);
            string tempFile = HttpContext.Current.Server.MapPath("~" + file);
            string fname = Path.GetFileName(tempFile);
            HttpResult result = uploader.UploadFile(tempFile, fname, token);
            string url = "http://7xw2i7.com1.z0.glb.clouddn.com/" + fname;
            return new ResultData
            {
                Code = result.Code == 200 ? 0 : result.Code,
                Msg = result.RefText,
                Data = url,
            };
        }
    }
}