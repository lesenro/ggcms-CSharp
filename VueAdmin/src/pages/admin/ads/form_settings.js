import imageUpload from "@/components/imageUpload";

export const defaultValue = {
    // "createTime": "2019-04-16T05:20:04.324Z",
    "Id": 0,
    "Content": "",
    "Describe": "",
    "GroupKey": "",
    "Image": "",
    "OrderID": 0,
    "Status": true,
    "Title": "",
    "Url": ""
};
export default {
    props: {
        "label-width": "100px",
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
                            key: "Title",
                            name: "广告标题",
                            type: "text",
                            itemProps: {
                                rules: [
                                    { required: true, message: "请输入广告标题", trigger: "blur" },
                                    { min: 1, max: 100, message: "长度在 1 到 100 个字符", trigger: "blur" }
                                ],

                            },
                            controlProps: {

                            }
                        },

                        {
                            key: "GroupKey",
                            name: "广告分组",
                            type: "select",
                            itemProps: {
                                rules: [
                                    { required: true, message: "请选择广告分组", trigger: "blur" },
                                ],
                            },
                        },
                        {
                            key: "Url",
                            name: "跳转URL",
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
                            key: "Image",
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
                            key: "Describe",
                            name: "描述信息",
                            type: "textarea",
                            itemProps: {
                                rules: [
                                ],
                            },
                            controlProps: {
                            }
                        },
                        {
                            key: "OrderID",
                            name: "排序",
                            type: "number",
                            itemProps: {},
                            controlProps: {
                                style: {
                                    width: "auto",
                                }

                            }
                        },
                        {
                            key: "Status",
                            name: "状态",
                            type: "switch",
                            itemProps: {},
                            controlProps: {}
                        },
                    ]
                },
                {
                    key: "tab-2",
                    name: "自定义内容",
                    type: "tab",
                    controls: [{
                        key: "Content",
                        name: "自定义内容",
                        type: "code",
                        itemProps: {},
                        controlProps: {
                            options: {
                                mode: 'text/html',
                            }
                        }
                    },]
                },
            ]
        },
    ],
}

