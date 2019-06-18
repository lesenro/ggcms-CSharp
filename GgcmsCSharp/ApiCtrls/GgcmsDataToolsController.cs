using GgcmsCSharp.Models;
using GgcmsCSharp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Routing;

namespace GgcmsCSharp.ApiCtrls
{
    public class GgcmsDataToolsController : ApiBaseController
    {
        #region 数据库初始化
        [HttpGet]
        public IHttpActionResult DbInit()
        {
            this.ClearAll();
            this.categoriesInit();
            this.dictInit();
            this.adminInit();
            this.adminPowers();
            this.adminMenus();
            this.adminStyle();
            this.adminConfigs();
            return Ok("数据库初始化完成");
        }
        private void ClearAll()
        {
            Dbctx.Database.ExecuteSqlCommand("TRUNCATE TABLE [GgcmsCategories]");
            Dbctx.Database.ExecuteSqlCommand("TRUNCATE TABLE [GgcmsDictionaries]");
            Dbctx.Database.ExecuteSqlCommand("TRUNCATE TABLE [GgcmsMembers]");
            Dbctx.Database.ExecuteSqlCommand("TRUNCATE TABLE [GgcmsPowers]");
            Dbctx.Database.ExecuteSqlCommand("TRUNCATE TABLE [GgcmsStyles]");
        }
        private void adminConfigs()
        {
            int order = 1;
            Dbctx.GgcmsDictionaries.AddRange(new List<GgcmsDictionaries>()
            {
               new GgcmsDictionaries()
               {
                     DictName="站点根网址",
                     DictKey="cfg_basehost",
                     DictValue="/",
                     GroupKey="system_configs",
                     DictDescribe="站点根网址",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="cfg_base",
                     OtherProperty=Tools.JsonSerialize(new {
                        type= "text",
                        itemProps= new {
                        rules = new List<dynamic>(){
                            //new
                            //{
                            //    required=true,
                            //    message="",
                            //    trigger="blur"
                            //},
                            new {
                                min=1,
                                max=255,
                                message="长度在 1 到 255 个字符",
                                trigger="blur"
                            }
                        } ,
                        controlProps=new List<dynamic>()
                        {
                            new
                            {
                                style=new
                                {
                                    width="auto"
                                }
                            }
                        }
                     }})
               },
               new GgcmsDictionaries()
                {
                     DictName="logo上传",
                     DictKey="cfg_logo",
                     DictValue="",
                     GroupKey="system_configs",
                     DictDescribe="logo上传",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="cfg_base",
                     OtherProperty=Tools.JsonSerialize(new {
                        type= "upload",
                        itemProps= new {
                        rules = new List<dynamic>(){
                            new {
                                min=1,
                                max=255,
                                message="长度在 3 到 40 个字符",
                                trigger="blur"
                            }
                        } ,
                        controlProps=new List<dynamic>()
                        {
                            new
                            {
                                style=new
                                {
                                    width="auto"
                                }
                            }
                        }
                     }})
                },
               new GgcmsDictionaries()
                {
                     DictName="主页链接名",
                     DictKey="cfg_indexname",
                     DictValue="首页",
                     GroupKey="system_configs",
                     DictDescribe="主页链接名",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="cfg_base",
                     OtherProperty=Tools.JsonSerialize(new {
                        type= "text",
                        itemProps= new {
                        rules = new List<dynamic>(){
                            new {
                                min=1,
                                max=40,
                                message="长度在 1 到 40 个字符",
                                trigger="blur"
                            }
                        } ,
                        controlProps=new List<dynamic>()
                        {
                        }
                     }})
                },
               new GgcmsDictionaries()
                {
                     DictName="网页主页链接",
                     DictKey="cfg_indexurl",
                     DictValue="",
                     GroupKey="system_configs",
                     DictDescribe="网页主页链接",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="cfg_base",
                     OtherProperty=Tools.JsonSerialize(new {
                        type= "text",
                        itemProps= new {
                        rules = new List<dynamic>(){
                            new {
                                min=1,
                                max=255,
                                message="长度在 1 到 255 个字符",
                                trigger="blur"
                            }
                        } ,
                        controlProps=new List<dynamic>()
                        {
                        }
                     }})
                },
               new GgcmsDictionaries()
                {
                     DictName="网站名称",
                     DictKey="cfg_webname",
                     DictValue="GGCMS 码农记忆",
                     GroupKey="system_configs",
                     DictDescribe="网站名称",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="cfg_base",
                     OtherProperty=Tools.JsonSerialize(new {
                        type= "text",
                        itemProps= new {
                        rules = new List<dynamic>(){
                            new {
                                min=1,
                                max=40,
                                message="长度在 1 到 40 个字符",
                                trigger="blur"
                            }
                        } ,
                        controlProps=new List<dynamic>()
                        {
                        }
                     }})
                },
               new GgcmsDictionaries()
                {
                     DictName="网站版权信息",
                     DictKey="cfg_powerby",
                     DictValue="<p>&copy; GGCMS 2010-2017. All Rights Reserved.</p><p><a href='http://www.miibeian.gov.cn/' target='_blank'>豫ICP备17010644号-1</a></p>",
                     GroupKey="system_configs",
                     DictDescribe="网站版权信息",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="cfg_base",
                     OtherProperty=Tools.JsonSerialize(new {
                        type= "editor",
                        itemProps= new {
                        rules = new List<dynamic>(){
                        },
                     }})
                },
               new GgcmsDictionaries()
                {
                     DictName="标题图默认宽度",
                     DictKey="cfg_ddimg_width",
                     DictValue="",
                     GroupKey="system_configs",
                     DictDescribe="标题图默认宽度",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="cfg_ui",
                     OtherProperty=Tools.JsonSerialize(new {
                        type= "number",
                        itemProps= new {
                        rules = new List<dynamic>(){
                        } ,
                        controlProps=new List<dynamic>()
                        {
                        }
                     }})
                },
               new GgcmsDictionaries()
                {
                     DictName="标题图默认高度",
                     DictKey="cfg_ddimg_height",
                     DictValue="0",
                     GroupKey="system_configs",
                     DictDescribe="标题图默认高度",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="cfg_ui",
                     OtherProperty=Tools.JsonSerialize(new {
                        type= "number",
                        itemProps= new {
                        rules = new List<dynamic>(){
                        } ,
                        controlProps=new List<dynamic>()
                        {
                        }
                     }})
                },
               new GgcmsDictionaries()
                {
                     DictName="默认风格",
                     DictKey="cfg_default_style",
                     DictValue="default",
                     GroupKey="system_configs",
                     DictDescribe="默认风格",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="cfg_ui",
                     OtherProperty=Tools.JsonSerialize(new {
                        type= "select",
                        itemProps= new {
                        rules = new List<dynamic>(){
                        } ,
                        controlProps=new List<dynamic>()
                        {
                        }
                     }})
                },
               new GgcmsDictionaries()
                {
                     DictName="首页模板",
                     DictKey="cfg_template_home",
                     DictValue="index_main.cshtml",
                     GroupKey="system_configs",
                     DictDescribe="首页模板",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="cfg_ui",
                     OtherProperty=Tools.JsonSerialize(new {
                        type= "select",
                        itemProps= new {
                        rules = new List<dynamic>(){
                        } ,
                        controlProps=new List<dynamic>()
                        {
                        }
                     }})
                },
               new GgcmsDictionaries()
                {
                     DictName="栏目页模板",
                     DictKey="cfg_template_list",
                     DictValue="list_1.cshtml",
                     GroupKey="system_configs",
                     DictDescribe="栏目页模板",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="cfg_ui",
                     OtherProperty=Tools.JsonSerialize(new {
                        type= "select",
                        itemProps= new {
                        rules = new List<dynamic>(){
                        } ,
                        controlProps=new List<dynamic>()
                        {
                        }
                     }})
                },
               new GgcmsDictionaries()
                {
                     DictName="文章页模板",
                     DictKey="cfg_template_view",
                     DictValue="view_1.cshtml",
                     GroupKey="system_configs",
                     DictDescribe="文章页模板",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="cfg_ui",
                     OtherProperty=Tools.JsonSerialize(new {
                        type= "select",
                        itemProps= new {
                        rules = new List<dynamic>(){
                        } ,
                        controlProps=new List<dynamic>()
                        {
                        }
                     }})
                },
               new GgcmsDictionaries()
                {
                     DictName="是否启用移动端模板",
                     DictKey="cfg_mob_enable",
                     DictValue="False",
                     GroupKey="system_configs",
                     DictDescribe="是否启用移动端模板",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="cfg_ui",
                     OtherProperty=Tools.JsonSerialize(new {
                        type= "switch",
                        itemProps= new {
                        rules = new List<dynamic>(){
                        } ,
                        controlProps=new List<dynamic>()
                        {
                        }
                     }})
                },
               new GgcmsDictionaries()
                {
                     DictName="移动端识别标识",
                     DictKey="cfg_mob_flag",
                     DictValue="Iphone|iPod|Mobile|Android|Opera Mini|BlackBerry|webOS|UCWEB|Blazer|PSP",
                     GroupKey="system_configs",
                     DictDescribe="移动端识别标识",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="cfg_ui",
                     OtherProperty=Tools.JsonSerialize(new {
                        type= "text",
                        itemProps= new {
                        rules = new List<dynamic>(){
                            new {
                                min=1,
                                max=200,
                                message="长度在 1 到 200 个字符",
                                trigger="blur"
                            }
                        } ,
                        controlProps=new List<dynamic>()
                        {
                        }
                     }})
                },
               new GgcmsDictionaries()
                {
                     DictName="移动端首页模板",
                     DictKey="cfg_template_m_home",
                     DictValue="m_index_main.cshtml",
                     GroupKey="system_configs",
                     DictDescribe="移动端首页模板",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="cfg_ui",
                     OtherProperty=Tools.JsonSerialize(new {
                        type= "select",
                        itemProps= new {
                        rules = new List<dynamic>(){
                        } ,
                        controlProps=new List<dynamic>()
                        {
                        }
                     }})
                },
               new GgcmsDictionaries()
                {
                     DictName="移动端栏目页模板",
                     DictKey="cfg_template_m_list",
                     DictValue="m_list_22.cshtml",
                     GroupKey="system_configs",
                     DictDescribe="移动端栏目页模板",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="cfg_ui",
                     OtherProperty=Tools.JsonSerialize(new {
                        type= "select",
                        itemProps= new {
                        rules = new List<dynamic>(){
                        } ,
                        controlProps=new List<dynamic>()
                        {
                        }
                     }})
                },
               new GgcmsDictionaries()
                {
                     DictName="移动端文章页模板",
                     DictKey="cfg_template_m_view",
                     DictValue="m_view_aaaaa.cshtml",
                     GroupKey="system_configs",
                     DictDescribe="移动端文章页模板",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="cfg_ui",
                     OtherProperty=Tools.JsonSerialize(new {
                        type= "select",
                        itemProps= new {
                        rules = new List<dynamic>(){
                        } ,
                        controlProps=new List<dynamic>()
                        {
                        }
                     }})
                },
               new GgcmsDictionaries()
                {
                     DictName="每页记录数",
                     DictKey="cfg_page_size",
                     DictValue="10",
                     GroupKey="system_configs",
                     DictDescribe="每页显示记录数",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="cfg_content",
                     OtherProperty=Tools.JsonSerialize(new {
                        type= "number",
                        itemProps= new {
                        rules = new List<dynamic>(){
                        } ,
                        controlProps=new List<dynamic>()
                        {
                        }
                     }})
                },
               new GgcmsDictionaries()
                {
                     DictName="存储方式",
                     DictKey="cfg_uploadmode",
                     DictValue="local",
                     GroupKey="system_configs",
                     DictDescribe="存储方式",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="cfg_content",
                     OtherProperty=Tools.JsonSerialize(new {
                        type= "select",
                        itemProps= new {
                        rules = new List<dynamic>(){
                        } ,
                        controlProps=new List<dynamic>()
                        {
                        }
                     }})
                },

               new GgcmsDictionaries()
                {
                     DictName="APP_ACCESS_KEY",
                     DictKey="cfg_access_key",
                     DictValue="",
                     GroupKey="system_configs",
                     DictDescribe="三方应用appkey",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="cfg_content",
                     OtherProperty=Tools.JsonSerialize(new {
                        type= "text",
                        itemProps= new {
                        rules = new List<dynamic>(){
                            new {
                                min=1,
                                max=100,
                                message="长度在 1 到 100 个字符",
                                trigger="blur"
                            }
                        } ,
                        controlProps=new List<dynamic>()
                        {
                        }
                     }})
                },

               new GgcmsDictionaries()
                {
                     DictName="APP_SECRET_KEY",
                     DictKey="cfg_secret_key",
                     DictValue="",
                     GroupKey="system_configs",
                     DictDescribe="三方应用秘钥",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="cfg_content",
                     OtherProperty=Tools.JsonSerialize(new {
                        type= "text",
                        itemProps= new {
                        rules = new List<dynamic>(){
                            new {
                                min=1,
                                max=100,
                                message="长度在 1 到 100 个字符",
                                trigger="blur"
                            }
                        } ,
                        controlProps=new List<dynamic>()
                        {
                        }
                     }})
                },

               new GgcmsDictionaries()
                {
                     DictName="上传空间(bucket)",
                     DictKey="cfg_bucket",
                     DictValue="ggcms",
                     GroupKey="system_configs",
                     DictDescribe="上传空间(bucket)",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="cfg_content",
                     OtherProperty=Tools.JsonSerialize(new {
                        type= "text",
                        itemProps= new {
                        rules = new List<dynamic>(){
                        } ,
                        controlProps=new List<dynamic>()
                        {
                        }
                     }})
                },

               new GgcmsDictionaries()
                {
                     DictName="链接格式",
                     DictKey="cfg_link_template",
                     DictValue="",
                     GroupKey="system_configs",
                     DictDescribe="内链链接格式",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="cfg_content",
                     OtherProperty=Tools.JsonSerialize(new {
                        type= "text",
                        itemProps= new {
                        rules = new List<dynamic>(){
                             new {
                                min=1,
                                max=255,
                                message="长度在 1 到 255 个字符",
                                trigger="blur"
                            }
                        } ,
                        controlProps=new List<dynamic>()
                        {
                        }
                     }})
                },

               new GgcmsDictionaries()
                {
                     DictName="启用站内关键词",
                     DictKey="cfg_artkey_enable",
                     DictValue="True",
                     GroupKey="system_configs",
                     DictDescribe="是否启用站内关键词",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="cfg_other",
                     OtherProperty=Tools.JsonSerialize(new {
                        type= "switch",
                        itemProps= new {
                        rules = new List<dynamic>(){
                        } ,
                        controlProps=new List<dynamic>()
                        {
                        }
                     }})
                },
               new GgcmsDictionaries()
                {
                     DictName="每篇文章替换几个",
                     DictKey="cfg_artkey_rn",
                     DictValue="5",
                     GroupKey="system_configs",
                     DictDescribe="每篇文章替换几个",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="cfg_other",
                     OtherProperty=Tools.JsonSerialize(new {
                        type= "number",
                        itemProps= new {
                        rules = new List<dynamic>(){
                        } ,
                        controlProps=new List<dynamic>()
                        {
                        }
                     }})
                },
            });
            Dbctx.SaveChanges();

        }
        private void adminStyle()
        {
            Dbctx.GgcmsStyles.Add(new GgcmsStyles()
            {
                StyleName = "默认风格",
                Folder = "default",
                Descrip = "默认风格"
            });
            Dbctx.SaveChanges();

        }
        private void adminMenus()
        {
            int order = 1;
            Dbctx.SaveChanges();
            Dbctx.GgcmsPowers.AddRange(new List<GgcmsPowers>() {
                new GgcmsPowers()
                {
                    PowerName = "权限管理",
                    OrderId = order++,
                    PowerType = 1,
                },
                new GgcmsPowers()
                {
                    PowerName = "系统工具",
                    OrderId = order++,
                    PowerType = 1,
                },
                new GgcmsPowers()
                {
                    PowerName = "文件管理",
                    OrderId = order++,
                    PowerType = 1,
                },
                new GgcmsPowers()
                {
                    PowerName = "用户管理",
                    OrderId = order++,
                    PowerType = 1,
                },
                new GgcmsPowers()
                {
                    PowerName = "角色管理",
                    OrderId = order++,
                    PowerType = 1,
                },
                new GgcmsPowers()
                {
                    PowerName = "系统字典",
                    OrderId = order++,
                    PowerType = 1,
                },
                new GgcmsPowers()
                {
                    PowerName = "基本信息",
                    OrderId = order++,
                    PowerType = 1,
                },
                new GgcmsPowers()
                {
                    PowerName = "广告管理",
                    OrderId = order++,
                    PowerType = 1,
                },
                new GgcmsPowers()
                {
                    PowerName = "友情链接",
                    OrderId = order++,
                    PowerType = 1,
                },
                new GgcmsPowers()
                {
                    PowerName = "全站标签",
                    OrderId = order++,
                    PowerType = 1,
                },
                new GgcmsPowers()
                {
                    PowerName = "代码片段",
                    OrderId = order++,
                    PowerType = 1,
                },
                new GgcmsPowers()
                {
                    PowerName = "分类导航",
                    OrderId = order++,
                    PowerType = 1,
                },
                new GgcmsPowers()
                {
                    PowerName = "专题管理",
                    OrderId = order++,
                    PowerType = 1,
                },
                new GgcmsPowers()
                {
                    PowerName = "单页文章",
                    OrderId = order++,
                    PowerType = 1,
                },
                new GgcmsPowers()
                {
                    PowerName = "文章管理",
                    OrderId = order++,
                    PowerType = 1,
                },
                new GgcmsPowers()
                {
                    PowerName = "风格模板",
                    OrderId = order++,
                    PowerType = 1,
                },
                new GgcmsPowers()
                {
                    PowerName = "任务管理",
                    OrderId = order++,
                    PowerType = 1,
                },
                new GgcmsPowers()
                {
                    PowerName = "SQL语句",
                    OrderId = order++,
                    PowerType = 1,
                },
                new GgcmsPowers()
                {
                    PowerName = "采集规则",
                    OrderId = order++,
                    PowerType = 1,
                },
            });
            Dbctx.SaveChanges();

        }
        private void adminPowers()
        {
            int order = 1;
            //Get the executing assembly
            var assembly = Assembly.GetExecutingAssembly();

            //Get all classes that inherit from the Controller class that are public and not abstract
            //Replace Controller with ApiController for Web Api
            var types = assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(ApiController)) && t.IsPublic && !t.IsAbstract);
            foreach (var type in types)
            {
                if (type.BaseType.Name != "ApiBaseController")
                {
                    continue;
                }
                //Get the Controller Name minus the word Controller
                string controllerName = type.Name
                .Substring(0, type.Name.IndexOf("Controller", System.StringComparison.InvariantCulture));

                //Get all Methods within the inherited class
                var methods = type.GetMethods()
                .Where(x => x.IsPublic && x.DeclaringType.Equals(type));
                foreach (var m in methods)
                {
                    var power = new GgcmsPowers()
                    {
                        Path = controllerName + "/" + m.Name,
                        PowerName = m.Name,
                        OrderId=order++,
                        PowerType=0,
                    };
                    foreach(var attr in m.CustomAttributes)
                    {
                        if (attr.AttributeType.Name == "DisplayNameAttribute"&&attr.ConstructorArguments.Count>0)
                        {
                            power.PowerName = attr.ConstructorArguments[0].Value.ToString();
                        }
                    }

                    Dbctx.GgcmsPowers.Add(power);
                }
            }
            Dbctx.SaveChanges();
        }

        private void adminInit()
        {
            Dbctx.GgcmsMembers.AddRange(new List<GgcmsMembers>() {
                new GgcmsMembers()
                {
                    UserName="admin",
                    PassWord=Tools.getMd5Hash("123456"),
                    JoinTime=DateTime.Now,
                }
            });
            Dbctx.SaveChanges();
        }
        private void dictInit()
        {
            int order = 1;
            Dbctx.GgcmsDictionaries.AddRange(new List<GgcmsDictionaries>() {
                new GgcmsDictionaries()
                {
                     DictName="本地上传",
                     DictKey="local",
                     DictValue="local",
                     GroupKey="upload_type",
                     DictDescribe="本地文件上传",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="",
                     OtherProperty=""
                },
                new GgcmsDictionaries()
                {
                     DictName="七牛上传",
                     DictKey="qiniu",
                     DictValue="qiniu",
                     GroupKey="upload_type",
                     DictDescribe="七牛文件上传",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="",
                     OtherProperty=""
                },
                new GgcmsDictionaries()
                {
                     DictName="广告分组",
                     DictKey="ads_group",
                     DictValue="",
                     GroupKey="base_dict",
                     DictDescribe="广告分组",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="",
                     OtherProperty=""
                },
                new GgcmsDictionaries()
                {
                     DictName="友链类型",
                     DictKey="link_type",
                     DictValue="",
                     GroupKey="base_dict",
                     DictDescribe="友链类型",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="",
                     OtherProperty=""
                },
                new GgcmsDictionaries()
                {
                     DictName="一级",
                     DictKey="l_1",
                     DictValue="1",
                     GroupKey="top_level",
                     DictDescribe="置顶一级",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="",
                     OtherProperty=""
                },
                new GgcmsDictionaries()
                {
                     DictName="二级",
                     DictKey="l_2",
                     DictValue="2",
                     GroupKey="top_level",
                     DictDescribe="置顶二级",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="",
                     OtherProperty=""
                },
                new GgcmsDictionaries()
                {
                     DictName="三级",
                     DictKey="l_3",
                     DictValue="3",
                     GroupKey="top_level",
                     DictDescribe="置顶三级",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="",
                     OtherProperty=""
                },
                new GgcmsDictionaries()
                {
                     DictName="四级",
                     DictKey="l_4",
                     DictValue="4",
                     GroupKey="top_level",
                     DictDescribe="置顶四级",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="",
                     OtherProperty=""
                },
                new GgcmsDictionaries()
                {
                     DictName="五级",
                     DictKey="l_5",
                     DictValue="5",
                     GroupKey="top_level",
                     DictDescribe="置顶五级",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="",
                     OtherProperty=""
                },
                new GgcmsDictionaries()
                {
                     DictName="首页推荐",
                     DictKey="disp_home",
                     DictValue="1",
                     GroupKey="display_mode",
                     DictDescribe="首页显示",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="",
                     OtherProperty=""
                },
                new GgcmsDictionaries()
                {
                     DictName="分类推荐",
                     DictKey="disp_category",
                     DictValue="2",
                     GroupKey="display_mode",
                     DictDescribe="分类推荐",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="",
                     OtherProperty=""
                },
                new GgcmsDictionaries()
                {
                     DictName="内容页推荐",
                     DictKey="disp_content",
                     DictValue="3",
                     GroupKey="display_mode",
                     DictDescribe="内容页推荐",
                     OrderId=order++,
                     DictStatus=1,
                     DictType=0,
                     ParentKey="",
                     OtherProperty=""
                },
            });
            Dbctx.SaveChanges();
        }
        private void categoriesInit()
        {
            Dbctx.GgcmsCategories.AddRange(new List<GgcmsCategories>() {
                new GgcmsCategories()
                {
                     CategoryName="首页",
                     RedirectUrl="/"
                }
            });
            Dbctx.SaveChanges();
        }
        #endregion
    }
}
