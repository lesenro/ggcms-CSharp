using GgcmsCSharp.ApiCtrls;
using GgcmsCSharp.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;

namespace GgcmsCSharp.Models
{
    public class UpFileClass
    {
        public string filePath { get; set; }
        public int fileType { get; set; }
        public string propertyName { get; set; }
        private static IThirdPartyStorage getFileSaveHandler()
        {
            Dictionary<string, string> cfgs = CacheHelper.GetSysConfigs();
            switch (cfgs["cfg_uploadmode"])
            {
                case "local":
                    return new LocalSave();
                case "qiniu":
                    return new QiniuHandle(cfgs["cfg_basehost"]);
                default:
                    return new LocalSave();
            }
        }
        public static string FileSave(string file,string cnt,int ftype)
        {
            IThirdPartyStorage fileStorage = getFileSaveHandler();
            ResultInfo result = fileStorage.UploadFile(file);
            string rval = cnt;
            switch (ftype)
            {
                case 1:
                    rval = cnt.Replace(fileStorage.serverUrl + file, result.Data);
                    break;
                default:
                    rval = result.Data;
                    break;
            }
            return rval;
        }
        public static T FileSave<T>(T tinfo, List<UpFileClass> files)
        {
            Type t = tinfo.GetType();
            IThirdPartyStorage fileStorage = getFileSaveHandler();

            foreach (UpFileClass ufile in files)
            {
                PropertyInfo pinfo = t.GetProperty(ufile.propertyName);

                ResultInfo result = fileStorage.UploadFile(ufile.filePath);
                switch (ufile.fileType)
                {
                    case 1:
                        string cnt = pinfo.GetValue(tinfo).ToString();
                        pinfo.SetValue(tinfo, cnt.Replace(fileStorage.serverUrl + ufile.filePath, result.Data));
                        break;
                    case 2:
                        List<GgcmsAttachments> attachments = (List<GgcmsAttachments>)pinfo.GetValue(tinfo);
                        foreach (GgcmsAttachments attach in attachments)
                        {
                            if (attach.RealName == ufile.filePath)
                            {
                                attach.AttaUrl = result.Data;
                                attach.CreateTime = DateTime.Now;
                            }
                        }
                        break;
                    default:
                        pinfo.SetValue(tinfo, result.Data);
                        break;
                }
            }
            return tinfo;
        }
    }
}