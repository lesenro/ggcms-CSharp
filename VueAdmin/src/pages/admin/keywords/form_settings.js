
export const defaultValue = {
    // "createTime": "2019-04-16T05:20:04.324Z",
    "Id": 0,
    "Keyword": "",
    "Url": "",
    "Describe": "",
    "Status": true,
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
                    key: "Keyword",
                    name: "关键词",
                    type: "text",
                    itemProps: {
                        rules: [
                            { required: true, message: "请输入关键词", trigger: "blur" },
                            { min: 1, max: 100, message: "长度在 1 到 100 个字符", trigger: "blur" }
                        ],

                    },
                    controlProps: {
                    }
                },

                {
                    key: "Url",
                    name: "URL地址",
                    type: "text",
                    itemProps: {
                        rules: [
                            { required: true, message: "请输入URL地址", trigger: "blur" },
                        ],
                    },
                    controlProps: {
                    }
                },
                {
                    key: "Describe",
                    name: "描述",
                    type: "textarea",
                    itemProps: {},
                    controlProps: {}
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

