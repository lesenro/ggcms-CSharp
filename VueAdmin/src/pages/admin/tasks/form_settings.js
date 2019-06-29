import imageUpload from "@/components/imageUpload";

export const defaultValue = {
    // "createTime": "2019-04-16T05:20:04.324Z",
    "Id": 0,
    "TaskName": "",
    "TaskType": 0,
    "TaskConfigs": "",
    "Status": 0,
    "Switch": 0,
    "PlanType": 0,
    "RunTime": "",
    "PlanOptions": "",
};
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
                    options:TaskType,
                    itemProps: {
                        rules: [
                            { required: true, message: "请选择广告分组", trigger: "blur" },
                        ],
                    },
                },
            ]
        },
    ],
}

