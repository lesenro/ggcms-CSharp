
export const defaultValue = {
    // "createTime": "2019-04-16T05:20:04.324Z",
    "Id": 0,
    "WebName": "",
    "Url": "",
    "LinkType": "",
    "LogoImg": "",
    "OrderId": 0,
    "Status": true,
    "ExtAttrib": "",
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
            type: "div",
            controls: [
                {
                    key: "WebName",
                    name: "网站名称",
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
                    key: "LinkType",
                    name: "链接类型",
                    type: "select",
                    itemProps: {
                        rules: [
                            { required: true, message: "请选择广告分组", trigger: "blur" },
                        ],
                    },
                },
                {
                    key: "Url",
                    name: "网站地址",
                    type: "text",
                    itemProps: {
                        rules: [
                            { required: true, message: "请输入网站地址", trigger: "blur" },
                        ],
                    },
                    controlProps: {
                    }
                },
                {
                    key: "LogoImg",
                    name: "图片上传",
                    type: "image-upload",
                    itemProps: {
                        rules: [
                        ],
                    },
                    controlProps: {
                        action: ""
                    }
                },
                {
                    key: "ExtAttrib",
                    name: "其他信息",
                    type: "textarea",
                    itemProps: {
                        rules: [
                        ],
                    },
                    controlProps: {
                    }
                },
                {
                    key: "OrderId",
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
    ],
}

