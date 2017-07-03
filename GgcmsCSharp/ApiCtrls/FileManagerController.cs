using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Configuration;
using System.Text;

namespace GgcmsCSharp.ApiCtrls
{
    public class FileManager 
    {
        private string getLocalPath(string path)
        {
            string localPath = HttpContext.Current.Server.MapPath("~/" + path);
            return localPath;
        }
        // GET: api/GgcmsCategories
        [HttpGet]
        public ResultData GetList(string path)
        {
            try
            {
                string localPath = getLocalPath(path);
                List<string> dirlist = new List<string>();
                dirlist.AddRange(Directory.GetDirectories(localPath));
                List<string> filelist = new List<string>();
                filelist.AddRange(Directory.GetFiles(localPath));
                List<dynamic> files = new List<dynamic>();
                foreach (string s in dirlist)
                {
                    files.Add(new
                    {
                        name = Path.GetFileName(s),
                        type = "dir",
                    });
                }
                foreach (string s in filelist)
                {
                    files.Add(new
                    {
                        name = Path.GetFileName(s),
                        type = "file",
                    });
                }
                return new ResultData
                {
                    Code = 0,
                    Msg = "",
                    Data =new {
                        files=files,
                        path=path,
                    },
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

        // GET: api/FileManager/5
        public ResultData GetInfo(string file)
        {
            try
            {
                string filePath = getLocalPath(file); 
                string info = File.ReadAllText(filePath, Encoding.UTF8);
                return new ResultData
                {
                    Code = 0,
                    Msg = file,
                    Data = info,
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

        // PUT: api/FileManager/5
        public ResultData Save(string file, string value)
        {
            try
            {
                string filePath = getLocalPath(file);
                File.WriteAllText(filePath, value, Encoding.UTF8);
                return new ResultData
                {
                    Code = 0,
                    Msg = "ok",
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

        // DELETE: api/FileManager/5
        public ResultData Delete(string path, dynamic files)
        {
            try
            {
                foreach(dynamic f in files)
                {
                    string fn = f.name.ToString();
                    string ftype = f.type.ToString();

                    string filePath = getLocalPath(path + "/" + fn);
                    if (ftype == "dir")
                    {
                        Directory.Delete(filePath, true);
                    }
                    else
                    {
                        File.Delete(filePath);
                    }
                }
                return new ResultData
                {
                    Code = 0,
                    Msg = "ok",
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
        public ResultData NewDir(string path)
        {
            try
            {
                string filePath = getLocalPath(path);
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                    return new ResultData
                    {
                        Code = 0,
                        Msg = "ok",
                    };
                }
                else
                {
                    return new ResultData
                    {
                        Code = 2,
                        Msg = "文件夹已存在",
                    };
                }
                
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
        public ResultData StaticFileReName(string oldname,string newname)
        {
            try
            {
                oldname = getLocalPath(oldname);
                newname = getLocalPath(newname);

                if (Directory.Exists(oldname))
                {
                    if (Directory.Exists(newname))
                    {
                        return new ResultData
                        {
                            Code = 2,
                            Msg = "文件夹已存在",
                        };
                    }
                    else
                    {
                        Directory.Move(oldname, newname);
                    }
                }
                else if (File.Exists(oldname))
                {
                    if (File.Exists(newname))
                    {
                        return new ResultData
                        {
                            Code = 2,
                            Msg = "文件已存在",
                        };
                    }
                    else
                    {
                        File.Move(oldname, newname);
                    }
                }
                return new ResultData
                {
                    Code = 0,
                    Msg = "ok",
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
    }
}
