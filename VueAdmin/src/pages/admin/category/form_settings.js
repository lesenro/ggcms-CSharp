import imageUpload from "@/components/imageUpload";

export const defaultValue = {
    Id: 0,
    ParentId: 0,
    CategoryName: "",
    LogoImg: "",
    StyleName: "",
    TmplName: "",
    ArticleTmplName: "",
    MobileTmplName: "",
    ArticleMobileTmplName: "",
    RedirectUrl: "",
    Keywords: "",
    Description: "",
    Content: "",
    ExtAttrib: "",
    RouteKey: "",
    OrderId: 0,
    PageSize: 10,
    ImgWidth: 0,
    ImgHeight: 0,
    CategoryType: 0,
    ExtModelId: 0,
};
export default {
    props: {
        "label-width": "150px",
        "size": "mini",
    },
    buttons: {
        hidden: true
    },
    layouts: [
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
                            key: "CategoryName",
                            name: "分类名称",
                            type: "text",
                            itemProps: {
                                rules: [
                                    { required: true, message: "请输入网站名称", trigger: "blur" },
                                    { min: 1, max: 100, message: "长度在 1 到 100 个字符", trigger: "blur" }
                                ],

                            },
                            controlProps: {

                            }
                        },
                        {
                            key: "RedirectUrl",
                            name: "跳转地址",
                            type: "text",
                            component: imageUpload,
                            itemProps: {
                                rules: [
                                ],
                            },
                            controlProps: {
                            }
                        },
                        {
                            key: "LogoImg",
                            name: "图片上传",
                            type: "upload",
                            component: imageUpload,
                            itemProps: {
                                rules: [
                                ],
                            },
                            controlProps: {
                                action: ""
                            }
                        },
                        {
                            key: "RouteKey",
                            name: "路由关键词",
                            type: "text",
                            component: imageUpload,
                            itemProps: {
                                rules: [
                                ],
                            },
                            controlProps: {
                            }
                        },
                        {
                            key: "ExtModelId",
                            name: "数据模型",
                            type: "select",
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
                            name: "分类模板",
                            type: "select",
                            itemProps: {
                            },
                        },
                        {
                            key: "ArticleTmplName",
                            name: "文章模板",
                            type: "select",
                            itemProps: {
                            },
                        },
                        {
                            key: "MobileTmplName",
                            name: "移动分类模板",
                            type: "select",
                            itemProps: {
                            },
                        },
                        {
                            key: "ArticleMobileTmplName",
                            name: "移动文章模板",
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
                            key: "PageSize",
                            name: "每页显示记录数",
                            type: "number",
                            itemProps: {
                            },
                        },
                        {
                            key: "Keywords",
                            name: "Keywords",
                            type: "textarea",
                            itemProps: {
                            },
                        },
                        {
                            key: "Description",
                            name: "Description",
                            type: "textarea",
                            itemProps: {
                            },
                        },
                        {
                            key: "ImgWidth",
                            name: "标题图宽",
                            type: "number",
                            itemProps: {
                            },
                        },
                        {
                            key: "ImgHeight",
                            name: "标题图高",
                            type: "number",
                            itemProps: {
                            },
                        },
                        {
                            key: "ExtAttrib",
                            name: "扩展属性",
                            type: "textarea",
                            itemProps: {
                                rules: [
                                ],
                            },
                            controlProps: {
                            }
                        },]
                },
                {
                    key: "tab-4",
                    name: "自定义内容",
                    type: "tab",
                    controls: [
                        {
                            key: "Content",
                            name: "自定义内容",
                            type: "editor",
                            itemProps: {
                                labelHidden: true
                            },
                            controlProps: {
                                style: {
                                    height: "320px",
                                    marginBottom: "50px",
                                }
                            }
                        },
                    ]
                }
            ],
        },
    ],
}

