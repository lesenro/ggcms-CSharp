export const defaultValue = {
    // "createTime": "2019-04-16T05:20:04.324Z",
    "Id": 0,
    "DictName": "",
    "GroupKey": "",
    "ParentKey": "",
    "DictKey": "",
    "DictValue": "",
    "OtherProperty": "",
    "DictDescribe": "",
    "DictType": 0,
    "OrderId": 0,
    "DictStatus": true,
};
export default {
    props: {
        "label-width": "100px"
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
                    key: "DictName",
                    name: "字典名称",
                    type: "text",
                    itemProps: {
                        rules: [
                            { required: true, message: "请输入字典名称", trigger: "blur" },
                            { min: 2, max: 40, message: "长度在 3 到 40 个字符", trigger: "blur" }
                        ],

                    },
                    controlProps: {
                        style: {
                            width: "auto",
                        }
                    }
                },
                {
                    key: "DictDescribe",
                    name: "类型描述",
                    type: "textarea",
                    itemProps: {},
                    controlProps: {}
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
            ]
        },
    ],
}



