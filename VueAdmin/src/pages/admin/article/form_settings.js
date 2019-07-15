
export class defaultValue {
    Id = 0;
    Content = "";
    Title = "";
    TitleImg = "";
    RedirectUrl = "";
    Source = "";
    SourceUrl = "";
    Keywords = "";
    Description = "";
    TmplName = "";
    StyleName = "";
    PageTitle = "";
    ExtModelId = 0;
    MobileTmplName = "";
    ShowType = 0;
    ShowLevel = 0;
    Author = "";
    CategoryId = [];
    attachments = [];
    files = [];
    pages = [];
    ModuleInfo = {
        Id: 0,
        Columns: [],
    };
}

export class ArticleForm {
    props = {
        "label-width": "150px",
        "size": "mini",
    };
    buttons = {
        hidden: true
    };
    layouts = [
        {
            key: "div",
            name: "div",
            type: "tabs",
            props: {
                value: "tab-1"
            },
            layouts: [
                {
                    key: "tab-1",
                    name: "基础信息",
                    type: "tab",
                    controls: [
                        {
                            key: "Title",
                            name: "文章标题",
                            type: "text",
                            itemProps: {
                                rules: [
                                    { required: true, message: "请输入文章标题", trigger: "blur" },
                                    { min: 1, max: 100, message: "长度在 1 到 100 个字符", trigger: "blur" }
                                ],

                            },
                            controlProps: {

                            }
                        },
                        {
                            key: "CategoryId",
                            name: "所属分类",
                            type: "cascader",
                            itemProps: {
                                rules: [
                                    { required: true, message: "请选择所属分类", trigger: "blur" },
                                ],
                            },
                            controlProps: {
                                props: {
                                    checkStrictly: true
                                }
                            }
                        },
                        {
                            key: "TitleImg",
                            name: "图片上传",
                            type: "image-upload",
                            itemProps: {
                                rules: [
                                ],
                                showClear:true,
                            },
                            controlProps: {
                                action: ""
                            }
                        },
                        {
                            key: "RedirectUrl",
                            name: "跳转地址",
                            type: "text",
                            itemProps: {
                                rules: [
                                ],
                            },
                            controlProps: {
                            }
                        },

                    ]
                },
                {
                    key: "tab-4",
                    name: "文章内容",
                    type: "tab",
                    controls: [
                        {
                            key: "pages",
                            name: "文章内容",
                            type: "article",
                            itemProps: {
                                labelHidden: true
                            },
                            controlProps: {
                            }
                        },
                    ]
                },
                {
                    key: "tab-2",
                    name: "显示相关",
                    type: "tab",
                    controls: [
                        {
                            key: "StyleName",
                            name: "风格",
                            type: "select",
                            itemProps: {
                            },
                        },
                        {
                            key: "TmplName",
                            name: "显示模板",
                            type: "select",
                            itemProps: {
                            },
                        },
                        {
                            key: "MobileTmplName",
                            name: "移动端模板",
                            type: "select",
                            itemProps: {
                            },
                        },
                        {
                            key: "ShowType",
                            name: "显示类型",
                            type: "select",
                            itemProps: {
                            },
                        },
                        {
                            key: "ShowLevel",
                            name: "置顶级别",
                            type: "select",
                            itemProps: {
                            },
                        },
                    ]
                },
                {
                    key: "tab-3",
                    name: "其他设置",
                    type: "tab",
                    controls: [
                        {
                            key: "Keywords",
                            name: "Keywords",
                            type: "textarea",
                            itemProps: {
                            },
                            controlProps:{
                                maxlength:255
                            }
                        },
                        {
                            key: "Description",
                            name: "Description",
                            type: "textarea",
                            itemProps: {
                            },
                            controlProps:{
                                maxlength:255
                            }
                        },
                        {
                            key: "Author",
                            name: "作者",
                            type: "text",
                            itemProps: {
                            },
                        },
                        {
                            key: "Source",
                            name: "来源",
                            type: "text",
                            itemProps: {
                                rules: [
                                ],
                            },
                            controlProps:{
                                maxlength:255
                            }
                        },
                        {
                            key: "SourceUrl",
                            name: "来源地址",
                            type: "text",
                            itemProps: {
                                rules: [
                                ],
                            },
                            controlProps:{
                                maxlength:255
                            }
                        },

                    ]
                },

            ],
        },
    ];
}

export class GgcmsAttachment {
    Id = 0;
    AttaTitle = "";
    AttaUrl = "";
    Describe = "";
    RealName = "";
    key = 0;
    form = {};
}


export class GgcmsAttachmentFrom {
    props = {
        "label-width": "150px",
        "size": "mini",
    };
    buttons = {
        hidden: true
    };
    layouts = [
        {
            key: "div",
            name: "div",
            type: "row",
            layouts: [
                {
                    key: "col-1",
                    name: "col-1",
                    type: "col",
                    props: {
                        span: 12
                    },
                    controls: [
                        {
                            key: "AttaTitle",
                            name: "附件标题",
                            type: "text",
                            itemProps: {
                                rules: [
                                    { required: true, message: "请输入附件标题", trigger: "blur" },
                                    { min: 1, max: 100, message: "长度在 1 到 100 个字符", trigger: "blur" }
                                ],

                            },
                            controlProps: {

                            }
                        },
                    ]
                }, {
                    key: "col-2",
                    name: "col-2",
                    type: "col",
                    props: {
                        span: 12
                    },
                    controls: [
                        {
                            key: "AttaUrl",
                            name: "附件上传",
                            type: "file-upload",
                            itemProps: {
                                rules: [
                                    { required: true, message: "请上传附件", trigger: "blur" },
                                ],

                            },
                            controlProps: {
                                "show-file-list": false,
                                action: ""
                            }
                        },
                    ]
                }
            ],

        }
    ]
}


