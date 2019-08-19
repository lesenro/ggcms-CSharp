export const defaultValue = {
    // "createTime": "2019-04-16T05:20:04.324Z",
    "Id": 0,
    "ModuleName": "",
    "Describe": "",
    "TableName": "",
    "ViewName": "",
    "Columns": [],
    "inputType": "",
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
                    key: "ModuleName",
                    name: "模型名称",
                    type: "text",
                    itemProps: {
                        rules: [
                            { required: true, message: "请输入模型名称", trigger: "blur" },
                            { min: 1, max: 100, message: "长度在 1 到 100 个字符", trigger: "blur" }
                        ],

                    },
                    controlProps: {

                    }
                },
                {
                    key: "Describe",
                    name: "其他信息",
                    type: "textarea",
                    itemProps: {
                        rules: [
                        ],
                    },
                    controlProps: {
                    }
                }
            ]
        },
    ],
}

export class moduleColumns {
    Id = 0;
    ColName = "";
    ColKey = "";
    ColTitle = "";
    ColType = "";
    Options = "";
    Length = 0;
    ColDecimal = 0;
    OrderId = 0;
    Module_Id = 0;
    Value = null;
    tmpId = 0;
    inputType = "";
}

export const inputTypes = [
    {
        label: "普通输入",
        value: "text"
    },
    {
        label: "密码",
        value: "password"
    },
    {
        label: "多行文本",
        value: "textarea"
    },
    {
        label: "日期时间",
        value: "datetime-picker"
    },
    {
        label: "日期",
        value: "date-picker"
    },
    {
        label: "时间",
        value: "time-picker"
    },
    {
        label: "数字",
        value: "number"
    },
    {
        label: "开关",
        value: "switch"
    },
    {
        label: "复选框",
        value: "checkbox"
    },
    {
        label: "多选框",
        value: "checkbox-group"
    },
    {
        label: "单选框",
        value: "radio-group"
    },
    {
        label: "选择列表",
        value: "select"
    },
    {
        label: "上传图片",
        value: "uploadImage"
    },
    {
        label: "上传文件",
        value: "uploadFile"
    },
    {
        label: "滑条",
        value: "slider"
    },
    {
        label: "评星",
        value: "rate"
    },
    {
        label: "级联选择器",
        value: "cascader"
    },
    {
        label: "富文本",
        value: "editor"
    },
    {
        label: "代码编辑",
        value: "code"
    },
]

export class moduleForm {
    props = {
        "label-width": "100px",
        "size": "mini",
    };
    buttons = {
        hidden: true
    };
    layouts = [
        {
            key: "div",
            name: "div",
            type: "div",
            controls: [
                {
                    key: "ColTitle",
                    name: "字段名称",
                    type: "text",
                    itemProps: {
                        rules: [
                            { required: true, message: "请输入字段名称", trigger: "blur" },
                            { min: 1, max: 100, message: "长度在 1 到 50 个字符", trigger: "blur" }
                        ],

                    },
                    controlProps: {

                    }
                },
                {
                    key: "ColKey",
                    name: "字段关键字",
                    type: "text",
                    itemProps: {
                        rules: [
                            { required: true, message: "请输入字段关键字", trigger: "blur" },
                            { min: 1, max: 100, message: "长度在 1 到 50 个字符", trigger: "blur" }
                        ],

                    },
                    controlProps: {

                    }
                },                
                {
                    key: "ColType",
                    name: "字段类型",
                    type: "select",
                    options: [
                        {
                            label: "字符串",
                            value: "nvarchar"
                        },
                        {
                            label: "文本",
                            value: "ntext"
                        },
                        {
                            label: "整数",
                            value: "int"
                        },
                        {
                            label: "长整数",
                            value: "bigint"
                        },
                        {
                            label: "日期时间",
                            value: "datetime"
                        },
                        {
                            label: "数字(有小数)",
                            value: "decimal"
                        },

                    ],
                    itemProps: {
                        rules: [
                            { required: true, message: "请选择字段类型", trigger: "blur" },
                        ],
                    },
                    controlProps: {
                    }
                },
                {
                    key: "inputType",
                    name: "输入方式",
                    type: "select",
                    options: inputTypes,
                    itemProps: {
                        rules: [
                            { required: true, message: "请选择字段类型", trigger: "blur" },
                        ],
                    },
                    controlProps: {
                    }
                },
                {
                    key: "Length",
                    name: "长度",
                    type: "number",
                    itemProps: {
                        rules: [
                        ],
                    },
                    controlProps: {
                    }
                },
                {
                    key: "ColDecimal",
                    name: "小数位",
                    type: "number",
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
                    itemProps: {
                        rules: [
                        ],
                    },
                    controlProps: {
                    }
                },

            ]
        },
    ];

}
export const validataOptions = [
    {
        label: "隐藏",
        value: "all",
        hidden: ["min", "max", "pattern"]
    },
    {
        label: "必选",
        value: "required",
        hidden: ["min", "max", "pattern"]
    },
    {
        label: "最小长度",
        value: "min",
        hidden: ["max", "pattern"]
    },
    {
        label: "最大长度",
        value: "max",
        hidden: ["min", "pattern"]
    },
    {
        label: "范围",
        value: "range",
        hidden: ["pattern"]
    },
    {
        label: "正则表达式",
        value: "regexp",
        hidden: ["min", "max"]
    },
];
export class controlValue {
    validata = "required";
    min = 0;
    max = 0;
    pattern = "";
    message = "";
}
export class controlForm {
    props = {
        "label-width": "100px",
        "size": "mini",
    };
    buttons = {
        hidden: true
    };
    layouts = [
        {
            key: "div",
            name: "div",
            type: "div",
            controls: [
                {
                    key: "validata",
                    name: "验证方式",
                    type: "select",
                    options: validataOptions.filter(x => x.value != "all"),
                    itemProps: {
                    },
                    controlProps: {
                    }
                },
                {
                    key: "min",
                    name: "最小长度",
                    type: "number",
                    itemProps: {
                        rules: [
                        ],
                    },
                    controlProps: {
                    }
                },
                {
                    key: "max",
                    name: "最大长度",
                    type: "number",
                    itemProps: {
                        rules: [
                        ],
                    },
                    controlProps: {
                    }
                },
                {
                    key: "pattern",
                    name: "正则表达式",
                    type: "text",
                    itemProps: {
                        rules: [
                        ],
                    },
                    controlProps: {
                    }
                },
                {
                    key: "message",
                    name: "提示信息",
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
    ];

}

export const propsOptions = [
    {
        label: "隐藏",
        value: "all",
        hidden: ["min", "max", "value", "size", "label-width"]
    },
    {
        label: "尺寸",
        value: "size",
        itemType: "item",
        hidden: ["min", "max", "value", "label-width"]
    },
    {
        label: "标签宽度",
        value: "label-width",
        itemType: "item",
        hidden: ["min", "max", "value", "size"]
    },
    {
        label: "最小长度",
        value: "minlength",
        itemType: "control",
        hidden: ["max", "value", "size", "label-width"]
    },
    {
        label: "最大长度",
        value: "maxlength",
        itemType: "control",
        hidden: ["min", "value", "size", "label-width"]
    },
    {
        label: "长度范围",
        value: "lenRange",
        itemType: "control",
        hidden: ["value", "size", "label-width"]
    },
    {
        label: "占位文字",
        value: "placeholder",
        itemType: "control",
        hidden: ["min", "max", "size", "label-width"]
    },

    {
        label: "最小值",
        value: "min",
        itemType: "control",
        hidden: ["max", "value", "size", "label-width"]
    },
    {
        label: "最大值",
        value: "max",
        itemType: "control",
        hidden: ["min", "value", "size", "label-width"]
    },
    {
        label: "范围",
        value: "range",
        itemType: "control",
        hidden: ["value", "size", "label-width"]
    },
]
export class propsValue {
    propType = "size";
    min = 0;
    max = 0;
    size = "";
    value = "";
    constructor() {
        this["label-width"] = 100;
    }
}
export class propsForm {
    props = {
        "label-width": "100px",
        "size": "mini",
    };
    buttons = {
        hidden: true
    };
    layouts = [
        {
            key: "div",
            name: "div",
            type: "div",
            controls: [
                {
                    key: "propType",
                    name: "常用属性",
                    type: "select",
                    options: propsOptions.filter(x => x.value != "all"),
                    itemProps: {
                    },
                    controlProps: {
                    }
                },
                {
                    key: "size",
                    name: "尺寸",
                    type: "select",
                    options: [
                        {
                            label: '缺省',
                            value: '',
                        },
                        {
                            label: '中等',
                            value: 'medium',
                        },
                        {
                            label: '较小',
                            value: 'small',
                        },
                        {
                            label: '迷你',
                            value: 'mini',
                        },
                    ],
                    itemProps: {
                        rules: [
                        ],
                    },
                    controlProps: {
                    }
                },
                {
                    key: "label-width",
                    name: "标签宽度",
                    type: "number",
                    itemProps: {
                        rules: [
                        ],
                    },
                    controlProps: {
                    }
                },
                {
                    key: "min",
                    name: "最小",
                    type: "number",
                    itemProps: {
                        rules: [
                        ],
                    },
                    controlProps: {
                    }
                },
                {
                    key: "max",
                    name: "最大",
                    type: "number",
                    itemProps: {
                        rules: [
                        ],
                    },
                    controlProps: {
                    }
                },
                {
                    key: "value",
                    name: "内容值",
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
    ];
}
