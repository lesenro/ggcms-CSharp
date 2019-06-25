
export const defaultValue = {
    // "createTime": "2019-04-16T05:20:04.324Z",
    "Id": 0,
    "UserName": "",
    "PassWord": "",
    "Sex": true,
    "Email": "",
    "Phone": "",
    "Roles_Id": "",
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
                    key: "UserName",
                    name: "用户名称:",
                    type: "text",
                    itemProps: {
                        rules: [
                            { required: true, message: "请输入用户名称", trigger: "blur" },
                            { min: 1, max: 100, message: "长度在 1 到 100 个字符", trigger: "blur" }
                        ],

                    },
                    controlProps: {

                    }
                },
                {
                    key: "PassWord",
                    name: "密码:",
                    type: "password",
                    itemProps: {
                        rules: [
                            { required: true, message: "请输入密码", trigger: "blur" },
                        ],
                    },
                    controlProps: {
                    }
                },
                {
                    key: "Roles_Id",
                    name: "角色:",
                    type: "select",
                    itemProps: {
                        rules: [
                        ],
                    },
                    controlProps: {
                    }
                },
                {
                    key: "Email",
                    name: "E-mail:",
                    type: "text",
                    itemProps: {
                        rules: [
                        ],
                    },
                    controlProps: {
                    }
                },
                {
                    key: "Phone",
                    name: "电话:",
                    type: "text",
                    itemProps: {},
                    controlProps: {

                    }
                },
                {
                    key: "Sex",
                    name: "性别:",
                    type: "switch",
                    itemProps: {},
                    controlProps: {
                        "active-color": "#409eff",
                        "inactive-color": "#ff4949",
                        "active-text": "男",
                        "inactive-text": "女",
                    }
                },
            ]
        },
    ],
}

