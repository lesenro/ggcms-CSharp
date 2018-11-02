namespace GgcmsCSharp.Migrations
{
    using GgcmsCSharp.Utils;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GgcmsCSharp.Models.GgcmsDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(GgcmsCSharp.Models.GgcmsDB context)
        {
            //  This method will be called after migrating to the latest version.
            if (context.GgcmsSysConfigs.Count() > 0)
            {
                return;
            }
            #region 系统字典
            context.GgcmsDictionaries.AddRange(new Models.GgcmsDictionary[] {new Models.GgcmsDictionary
                {
                    Title = "本地上传",
                    DictType = "-1",
                    OrderID = 1,
                    SysFlag = 1,
                    describe = "本地文件上传",
                    Value = "local"
                },
                new Models.GgcmsDictionary
                {
                    Title = "七牛上传",
                    DictType = "-1",
                    OrderID = 2,
                    SysFlag = 1,
                    describe = "七牛文件上传",
                    Value = "qiniu"
                }
                });
            context.SaveChanges();
            #endregion
            #region 权限管理
            #region 一级权限菜单
            int pid = 0, order = 1;
            var pows_l1 = new Models.GgcmsPower[]
            {
                new Models.GgcmsPower
                {
                    PowerName = "管理首页",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "home",
                    IconClass = "icon-home",
                    ShowInSidebar = true,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "home",

                },
                 new Models.GgcmsPower
                {
                    PowerName = "系统管理",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "system",
                    IconClass = "icon-settings",
                    ShowInSidebar = true,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "system",
                },
                 new Models.GgcmsPower
                {
                    PowerName = "网站设置",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "site",
                    IconClass = "icon-globe",
                    ShowInSidebar = true,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "site",

                },
                 new Models.GgcmsPower
                {
                    PowerName = "分类内容",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "contents",
                    IconClass = "icon-grid",
                    ShowInSidebar = true,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "contents",

                },
                 new Models.GgcmsPower
                {
                    PowerName = "风格模板",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "styles",
                    IconClass = "icon-layers",
                    ShowInSidebar = true,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "styles",

                }
                 ,
                 new Models.GgcmsPower
                {
                    PowerName = "任务管理",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "tasks",
                    IconClass = "icon-list",
                    ShowInSidebar = true,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "tasks",

                }
            };
            context.GgcmsPowers.AddRange(pows_l1);
            context.SaveChanges();

            #endregion
            #region 系统管理
            pid = pows_l1.Where(x => x.PowerTag == "system").FirstOrDefault().Id;
            var pows_system = new Models.GgcmsPower[]
            {
                new Models.GgcmsPower
                {
                    PowerName = "权限管理",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "power",
                    IconClass = "icon-check",
                    ShowInSidebar = true,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "power",

                }, new Models.GgcmsPower
                {
                    PowerName = "系统工具",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "tools",
                    IconClass = "icon-magic-wand",
                    ShowInSidebar = true,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "tools",

                }, new Models.GgcmsPower
                {
                    PowerName = "用户管理",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "member",
                    IconClass = "icon-people",
                    ShowInSidebar = true,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "member",

                }
                , new Models.GgcmsPower
                {
                    PowerName = "文件管理",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "files",
                    IconClass = "icon-folder-alt",
                    ShowInSidebar = true,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "files",

                } , new Models.GgcmsPower
                {
                    PowerName = "角色管理",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "roles",
                    IconClass = "icon-graduation",
                    ShowInSidebar = true,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "roles",

                }, new Models.GgcmsPower
                {
                    PowerName = "系统字典",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "dictionary",
                    IconClass = "icon-graduation",
                    ShowInSidebar = true,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "dictionary",

                }, new Models.GgcmsPower
                {
                    PowerName = "系统字典编辑",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "dictionaryEdit",
                    IconClass = "",
                    ShowInSidebar = false,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "dictionaryEdit",

                }
            };
            context.GgcmsPowers.AddRange(pows_system);
            context.SaveChanges();
            #endregion

            #region 网站设置
            pid = pows_l1.Where(x => x.PowerTag == "site").FirstOrDefault().Id;

            var pows_site = new Models.GgcmsPower[]
            {
                new Models.GgcmsPower
                {
                    PowerName = "基本信息",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "settings",
                    IconClass = "icon-wrench",
                    ShowInSidebar = true,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "settings",

                }, new Models.GgcmsPower
                {
                    PowerName = "广告管理",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "ads",
                    IconClass = "icon-star",
                    ShowInSidebar = true,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "ads",

                }, new Models.GgcmsPower
                {
                    PowerName = "友情链接",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "links",
                    IconClass = "icon-link",
                    ShowInSidebar = true,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "links",

                }, new Models.GgcmsPower
                {
                    PowerName = "全站标签",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "tags",
                    IconClass = "icon-tag",
                    ShowInSidebar = true,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "tags",

                }, new Models.GgcmsPower
                {
                    PowerName = "代码片段",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "codes",
                    IconClass = "fa fa-html5",
                    ShowInSidebar = true,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "codes",

                }
            };
            context.GgcmsPowers.AddRange(pows_site);
            context.SaveChanges();

            #endregion
            #region 内容导航
            pid = pows_l1.Where(x => x.PowerTag == "contents").FirstOrDefault().Id;
            var pows_contents = new Models.GgcmsPower[]
            {
                new Models.GgcmsPower
                {
                    PowerName = "分类导航",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "category",
                    IconClass = "icon-directions",
                    ShowInSidebar = true,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "category",

                }, new Models.GgcmsPower
                {
                    PowerName = "专题管理",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "topic",
                    IconClass = "icon-puzzle",
                    ShowInSidebar = true,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "topic",

                }, new Models.GgcmsPower
                {
                    PowerName = "文章管理",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "article",
                    IconClass = "icon-note",
                    ShowInSidebar = true,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "article",

                }, new Models.GgcmsPower
                {
                    PowerName = "单页文章",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "single",
                    IconClass = "icon-doc",
                    ShowInSidebar = true,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "single",

                }, new Models.GgcmsPower
                {
                    PowerName = "分类导航编辑",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "categoryEdit",
                    IconClass = "",
                    ShowInSidebar = false,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "categoryEdit",

                }, new Models.GgcmsPower
                {
                    PowerName = "文章编辑",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "articleEdit",
                    IconClass = "",
                    ShowInSidebar = false,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "articleEdit",

                }
            };
            context.GgcmsPowers.AddRange(pows_contents);
            context.SaveChanges();

            #endregion
            #region 任务管理
            pid = pows_l1.Where(x => x.PowerTag == "tasks").FirstOrDefault().Id;
            var pows_tasks = new Models.GgcmsPower[]
            {
                new Models.GgcmsPower
                {
                    PowerName = "SQL语句",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "sql",
                    IconClass = "fa fa-code",
                    ShowInSidebar = true,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "sql",

                }, new Models.GgcmsPower
                {
                    PowerName = "任务执行",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "runTask",
                    IconClass = "icon-equalizer",
                    ShowInSidebar = true,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "runTask",

                }, new Models.GgcmsPower
                {
                    PowerName = "采集规则",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "gather",
                    IconClass = "icon-cloud-download",
                    ShowInSidebar = true,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "gather",

                }
            };
            context.GgcmsPowers.AddRange(pows_tasks);
            context.SaveChanges();
            #endregion
            #region 风格模板
            pid = pows_l1.Where(x => x.PowerTag == "styles").FirstOrDefault().Id;
            var pows_styles = new Models.GgcmsPower[]
            {
                new Models.GgcmsPower
                {
                    PowerName = "风格编辑",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "stylesEdit",
                    IconClass = "",
                    ShowInSidebar = false,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "stylesEdit",

                },new Models.GgcmsPower
                {
                    PowerName = "模板浏览",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "template",
                    IconClass = "",
                    ShowInSidebar = false,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "template",

                },new Models.GgcmsPower
                {
                    PowerName = "静态文件",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "staticFile",
                    IconClass = "",
                    ShowInSidebar = false,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "staticFile",

                },new Models.GgcmsPower
                {
                    PowerName = "静态文件编辑",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "staticFileEdit",
                    IconClass = "",
                    ShowInSidebar = false,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "staticFileEdit",

                },new Models.GgcmsPower
                {
                    PowerName = "模板编辑",
                    OrderId = order++,
                    ParentId = pid,
                    PowerTag = "templateEdit",
                    IconClass = "",
                    ShowInSidebar = false,
                    PowerType = 0,
                    PowerParams = "",
                    Path = "templateEdit",

                }
            };
            context.GgcmsPowers.AddRange(pows_styles);
            context.SaveChanges();
            #endregion
            #endregion
            #region 系统设置
            order = 1;
            context.GgcmsSysConfigs.AddRange(new Models.GgcmsSysConfig[] {new Models.GgcmsSysConfig
                {
                 CfgName="cfg_basehost",
                 CfgValue="/",
                 Descrip="站点根网址",
                 GroupId=1,
                 Options="{\"type\":\"url\"}",
                 Protection=false,
                 OrderId = order++,
                }, new Models.GgcmsSysConfig
                {
                 CfgName="cfg_logo",
                 CfgValue="",
                 Descrip="站点logo",
                 GroupId=1,
                 Options="",
                 Protection=false,
                 OrderId = order++,
                }, new Models.GgcmsSysConfig
                {
                 CfgName="cfg_artkey_tmpl",
                 Descrip="关键词替换模板",
                 CfgValue="",
                 GroupId=1,
                 Options="",
                 Protection=false,
                 OrderId = order++,
                }, new Models.GgcmsSysConfig
                {
                 CfgName="cfg_uploadmode",
                 Descrip="存储方式",
                 CfgValue="",
                 GroupId=1,
                 Options="",
                 Protection=false,
                 OrderId = order++,
                }, new Models.GgcmsSysConfig
                {
                 CfgName="cfg_indexname",
                 Descrip="主页链接名",
                 CfgValue="",
                 GroupId=1,
                 Options="",
                 Protection=false,
                 OrderId = order++,
                }, new Models.GgcmsSysConfig
                {
                 CfgName="cfg_indexurl",
                 Descrip="网页主页链接",
                 CfgValue="",
                 GroupId=1,
                 Options="",
                 Protection=false,
                 OrderId = order++,
                }, new Models.GgcmsSysConfig
                {
                 CfgName="cfg_webname",
                 Descrip="网站名称",
                 CfgValue="",
                 GroupId=1,
                 Options="",
                 Protection=false,
                 OrderId = order++,
                }, new Models.GgcmsSysConfig
                {
                 CfgName="cfg_copyright",
                 Descrip="网站版权信息",
                 CfgValue="",
                 GroupId=1,
                 Options="",
                 Protection=false,
                 OrderId = order++,
                }, new Models.GgcmsSysConfig
                {
                 CfgName="cfg_ddimg_width",
                 Descrip="标题图默认宽度",
                 CfgValue="",
                 GroupId=1,
                 Options="",
                 Protection=false,
                 OrderId = order++,
                }, new Models.GgcmsSysConfig
                {
                 CfgName="cfg_ddimg_height",
                 Descrip="标题图默认高度",
                 CfgValue="",
                 GroupId=1,
                 Options="",
                 Protection=false,
                 OrderId = order++,
                }, new Models.GgcmsSysConfig
                {
                 CfgName="cfg_default_style",
                 Descrip="默认风格",
                 CfgValue="default",
                 GroupId=1,
                 Options="",
                 Protection=false,
                 OrderId = order++,
                }, new Models.GgcmsSysConfig
                {
                 CfgName="cfg_template_home",
                 Descrip="首页模板",
                 CfgValue="index_main.cshtml",
                 GroupId=1,
                 Options="",
                 Protection=false,
                 OrderId = order++,
                }, new Models.GgcmsSysConfig
                {
                 CfgName="cfg_template_list",
                 Descrip="栏目页模板",
                 CfgValue="list_1.cshtml",
                 GroupId=1,
                 Options="",
                 Protection=false,
                 OrderId = order++,
                }, new Models.GgcmsSysConfig
                {
                 CfgName="cfg_template_view",
                 Descrip="文章页模板",
                 CfgValue="view_1.cshtml",
                 GroupId=1,
                 Options="",
                 Protection=false,
                 OrderId = order++,
                }, new Models.GgcmsSysConfig
                {
                 CfgName="cfg_mob_enable",
                 Descrip="是否启用移动端模板",
                 CfgValue="False",
                 GroupId=1,
                 Options="",
                 Protection=false,
                 OrderId = order++,
                }, new Models.GgcmsSysConfig
                {
                 CfgName="cfg_mob_flag",
                 Descrip="Iphone|iPod|Mobile|Android|Opera Mini|BlackBerry|webOS|UCWEB|Blazer|PSP",
                 CfgValue="移动端识别标识",
                 GroupId=1,
                 Options="",
                 Protection=false,
                 OrderId = order++,
                }, new Models.GgcmsSysConfig
                {
                 CfgName="cfg_template_m_home",
                 Descrip="m_index_main.cshtml",
                 CfgValue="移动端首页模板",
                 GroupId=1,
                 Options="",
                 Protection=false,
                 OrderId = order++,
                }, new Models.GgcmsSysConfig
                {
                 CfgName="cfg_template_m_list",
                 Descrip="m_list_22.cshtml",
                 CfgValue="移动端栏目页模板",
                 GroupId=1,
                 Options="",
                 Protection=false,
                 OrderId = order++,
                }, new Models.GgcmsSysConfig
                {
                 CfgName="cfg_template_m_view",
                 Descrip="移动端文章页模板",
                 CfgValue="m_view_aaaaa.cshtml",
                 GroupId=1,
                 Options="",
                 Protection=false,
                 OrderId = order++,
                }, new Models.GgcmsSysConfig
                {
                 CfgName="cfg_access_key",
                 Descrip="APP_ACCESS_KEY",
                 CfgValue="",
                 GroupId=1,
                 Options="",
                 Protection=false,
                 OrderId = order++,
                }, new Models.GgcmsSysConfig
                {
                 CfgName="cfg_secret_key",
                 Descrip="APP_SECRET_KEY",
                 CfgValue="",
                 GroupId=1,
                 Options="",
                 Protection=false,
                 OrderId = order++,
                }, new Models.GgcmsSysConfig
                {
                 CfgName="cfg_bucket",
                 Descrip="上传空间(bucket)",
                 CfgValue="",
                 GroupId=1,
                 Options="",
                 Protection=false,
                 OrderId = order++,
                }, new Models.GgcmsSysConfig
                {
                 CfgName="cfg_link_template",
                 Descrip="链接格式",
                 CfgValue="",
                 GroupId=1,
                 Options="",
                 Protection=false,
                 OrderId = order++,
                }, new Models.GgcmsSysConfig
                {
                 CfgName="cfg_cache_enable",
                 Descrip="是否启用缓存",
                 CfgValue="",
                 GroupId=1,
                 Options="",
                 Protection=false,
                 OrderId = order++,
                }, new Models.GgcmsSysConfig
                {
                 CfgName="cfg_cache_dir",
                 Descrip="缓存页面保存路径",
                 CfgValue="",
                 GroupId=1,
                 Options="",
                 Protection=false,
                 OrderId = order++,
                }, new Models.GgcmsSysConfig
                {
                 CfgName="cfg_cache_timeout",
                 Descrip="缓存超时(分钟)",
                 CfgValue="",
                 GroupId=1,
                 Options="",
                 Protection=false,
                 OrderId = order++,
                }, new Models.GgcmsSysConfig
                {
                 CfgName="cfg_artkey_enable",
                 Descrip="是否启用站内关键词",
                 CfgValue="",
                 GroupId=1,
                 Options="",
                 Protection=false,
                 OrderId = order++,
                }, new Models.GgcmsSysConfig
                {
                 CfgName="cfg_artkey_rn",
                 Descrip="每篇文章替换几个",
                 CfgValue="",
                 GroupId=1,
                 Options="",
                 Protection=false,
                 OrderId = order++,
                }
            });
            context.SaveChanges();
            #endregion
            #region 添加管理员

            context.GgcmsMembers.Add(new Models.GgcmsMember {
                UserName = "admin",
                PassWord = Tools.getMd5Hash("123456"),
                Sex = false,
                Email = "",
                Scores = 0,
                Avatar = "",
                JoinTime = DateTime.Now,
                Level=0,
                Phone="",
                Roles_Id=0,
            });
            context.SaveChanges();
            #endregion
    }
}
}
