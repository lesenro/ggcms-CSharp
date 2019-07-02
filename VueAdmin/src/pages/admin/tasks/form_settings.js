import moment from 'moment'

export class defaultValue {
    // "createTime"= "2019-04-16T05=20=04.324Z";
    "Id" = 0;
    "TaskName" = "";
    "TaskType" = 3;
    "TaskConfigs" = "";
    "Status" = 0;
    "Switch" = true;
    "PlanType" = 1;
    "RunTime" = "";
    "PlanOptions" = "";
    //====以下任务执行选项，将保存在PlanOptions字段中
    "SpecificDate" = new Date();
    "StartDate" = new Date();
    "EndDate" = new Date();
    "IntervalMinute" = 1;
    "WeekDays" = [];
    "MonthDays" = []
}
export const TaskType = [
    // {
    //     label: "SQL任务",
    //     value: 1
    // },
    // {
    //     label: "采集任务",
    //     value: 2
    // },
    {
        label: "生成静态",
        value: 3
    },
];


export const TaskStatus =
    [
        {
            label: "未开始",
            value: 1
        },
        {
            label: "已结束",
            value: 2
        },
        {
            label: "准备就绪",
            value: 3
        },
        {
            label: "禁用",
            value: 4
        },
        {
            label: "执行中",
            value: 5
        },
    ];
export const PlanType =
    [
        {
            label: "执行1次",
            value: 1
        },
        {
            label: "每天",
            value: 2
        },
        {
            label: "每周",
            value: 3
        },
        {
            label: "每月",
            value: 4
        },
        {
            label: "时间段(分钟)",
            value: 5
        },
    ]
const MonthDays = function () {
    let days = [{
        label: "月未",
        value: 0
    },]
    for (let i = 1; i < 32; i++) {
        days.push({
            label: i.toString(),
            value: i
        });
    }
    return days;
}();
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
            type: "div",
            controls: [
                {
                    key: "TaskName",
                    name: "任务名称",
                    type: "text",
                    itemProps: {
                        rules: [
                            { required: true, message: "请输入任务名称", trigger: "blur" },
                            { min: 1, max: 100, message: "长度在 1 到 100 个字符", trigger: "blur" }
                        ],

                    },
                    controlProps: {

                    }
                },

                {
                    key: "TaskType",
                    name: "任务类型",
                    type: "select",
                    options: TaskType,
                    itemProps: {
                        rules: [
                            { required: true, message: "请选择任务类型", trigger: "blur" },
                        ],
                    },
                },
                {
                    key: "PlanType",
                    name: "执行方式",
                    type: "select",
                    options: PlanType,
                    itemProps: {
                        rules: [
                            { required: true, message: "请选择执行方式", trigger: "blur" },
                        ],
                    },
                },
                {
                    key: "SpecificDate",
                    name: "执行时间",
                    type: "datetime-picker",
                    itemProps: {
                        rules: [
                            { required: true, message: "请设置执行时间", trigger: "blur" },
                        ],
                    },
                },
                {
                    key: "StartDate",
                    name: "任务开始",
                    type: "datetime-picker",
                    itemProps: {
                        rules: [
                        ],
                    },
                    controlProps: {
                        clearable: true
                    }
                },
                {
                    key: "EndDate",
                    name: "任务结束",
                    type: "datetime-picker",
                    itemProps: {
                        rules: [
                        ],
                    },
                    controlProps: {
                        clearable: true
                    }
                },

                {
                    key: "IntervalMinute",
                    name: "间隔时间(分钟)",
                    type: "number",
                    itemProps: {
                        rules: [
                        ],
                    },
                    controlProps: {
                        min: 0,
                        max: 60 * 60 * 100
                    }
                },
                {
                    key: "WeekDays",
                    name: "每星期",
                    type: "select",
                    itemProps: {
                        rules: [
                        ],
                    },
                    options: [
                        {
                            label: "周日",
                            value: 0
                        },
                        {
                            label: "周一",
                            value: 1
                        },
                        {
                            label: "周二",
                            value: 2
                        },
                        {
                            label: "周三",
                            value: 3
                        },
                        {
                            label: "周四",
                            value: 4
                        },
                        {
                            label: "周五",
                            value: 5
                        },
                        {
                            label: "周六",
                            value: 6
                        },
                    ],
                    controlProps: {
                        multiple: true,
                        "collapse-tags": true,
                    }
                },
                {
                    key: "MonthDays",
                    name: "每月",
                    type: "select",
                    options: MonthDays,
                    itemProps: {
                        rules: [
                        ],
                    },
                    controlProps: {
                        multiple: true,
                        "collapse-tags": true,
                        clearable: true
                    }
                },
                {
                    key: "Switch",
                    name: "禁用/启用",
                    type: "switch",
                    itemProps: {
                        rules: [
                        ],
                    },
                    controlProps: {
                        "active-text": "启用",
                        "inactive-text": "禁用",
                        "inactive-color": "#ff4949"
                    }
                },
            ]
        },
    ],
}

export class StaticTask {
    //生成所有
    All = true;
    //要生成的分类列表
    Categories = [];
    //要生成的专题列表
    Topics = [];
}
export class StaticTaskForm {
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
            type: "div",
            controls: [
                {
                    key: "All",
                    name: "全部生成",
                    type: "switch",
                    itemProps: {
                        rules: [
                        ],
                    },
                },
                {
                    key: "Categories",
                    name: "生成分类",
                    type: "cascader",
                    itemProps: {
                        rules: [
                        ],
                    },
                    controlProps: {
                        props: {
                            multiple: true
                        },
                        "collapse-tags": true
                    }
                },
            ]
        }
    ];
}